﻿
using Consul;
using DataCore;
using DataCore.Entities;
using DataCore.Repositories;
using DataCore.Repository;
using EventCore.Interfaces;
using EventCore.RabbitMQEventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderService.Events;
using OrderService.Events.EventHandlers;
using RabbitMQ.Client;
using ServiceDiscovery;
using ServicesAPI.CustomerAPI;
using ServicesAPI.OrderAPI;
using System;
using System.Text;

namespace OrderServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddControllers();
            services.AddDbContext<CustomerOrderContext>(options =>
            {

                //var str = @"Data Source=(localdb)\MSSQLLocalDb;Initial Catalog=WebApp.Models.MultiTenantContext;Integrated Security=True";
                options.UseSqlServer(Configuration.GetConnectionString("Northwind"));
            });

            
            services.AddScoped<IOrderRepository,OrderRepository>();
            services.AddScoped<IOrderAPI, OrderAPI>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartDetailsRepository, CartDetailsRepository>();
            services.AddScoped<ICustomerService, CustomerService>();


            // configure strongly typed settings objects
            var authenticationProviderKey = "IdentityApiKey";
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var signingKey = Encoding.ASCII.GetBytes(appSettings.Secret);


            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(signingKey),
                ValidateIssuer = false,
                ValidIssuer = "http://localhost:7000",
                ValidateAudience = false,

            };

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = authenticationProviderKey;
                o.DefaultScheme = authenticationProviderKey;
            })
            .AddJwtBearer(authenticationProviderKey, x =>
            {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = tokenValidationParameters;
            });


            //Configure service bus
            //  services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMq"));
            // services.Configure<IRabbitMQPersistentConnection>(Configuration.GetSection("RabbitMq"));
            
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                      

                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBusConnection"],
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(Configuration["EventBusUserName"]))
                {
                    factory.UserName = Configuration["EventBusUserName"];
                }

                if (!string.IsNullOrEmpty(Configuration["EventBusPassword"]))
                {
                    factory.Password = Configuration["EventBusPassword"];
                }

                var retryCount = 5;
                if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                }

                return new DefaultRabbitMQPersistentConnection(factory, retryCount);
            });

            RegisterEventBus(services);
            services.Configure<ServiceDiscoveryConfiguration>(Configuration.GetSection("consulConfig"));

            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                //consul address  
                var address = "http://127.0.0.1:8500";

                consulConfig.Address = new Uri(address);

            }, null, handlerOverride =>
            {
                //disable proxy of httpclienthandler  
                handlerOverride.Proxy = null;
                handlerOverride.UseProxy = false;
            }));


            services.AddHttpClient();

            // Register Swagger  
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order API", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseConsul(Configuration);
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API V1");
            });

        }

        private void RegisterEventBus(IServiceCollection services)
        {
            //var subscriptionClientName = Configuration["SubscriptionClientName"];

           
           
                services.AddSingleton<IEventPublisher, RabbitMQPublisher>(sp =>
                {
                    var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                  //  var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                   // var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                 //   var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    var retryCount = 5;
                    if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                    {
                        retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                    }

                    return new RabbitMQPublisher(rabbitMQPersistentConnection, retryCount);
                });

            services.AddTransient<NewOrderStartedEvent>();
            services.AddHostedService<OrderFulfilledEventHandler>();
            // services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            //services.AddTransient<ProductPriceChangedIntegrationEventHandler>();
            //services.AddTransient<OrderStartedIntegrationEventHandler>();
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

           // eventBus.Subscribe<ProductPriceChangedIntegrationEvent, ProductPriceChangedIntegrationEventHandler>();
           // eventBus.Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();
        }
    }
}
