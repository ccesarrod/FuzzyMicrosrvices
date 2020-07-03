using Consul;
using DataCore;
using DataCore.Entities;
using DataCore.Repository;
using EventCore.RabbitMQEventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using ServiceDiscovery;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ProductService.Events.EventHandlers;
using RabbitMQ.Client;
using ServicesAPI.ProductAPI;
using System;
using System.Text;
using Microsoft.Extensions.Options;

namespace ProductService
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
            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddControllers();
            services.AddDbContext<CustomerOrderContext>(options =>
            {

                //var str = @"Data Source=(localdb)\MSSQLLocalDb;Initial Catalog=WebApp.Models.MultiTenantContext;Integrated Security=True";
                options.UseSqlServer(Configuration.GetConnectionString("Northwind"));
            });

            //services.AddScoped<IRepository<Category>, ICategoryRepository>();
          

            // configure strongly typed settings objects
            var  authenticationProviderKey= "IdentityApiKey";
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var signingKey = Encoding.ASCII.GetBytes(appSettings.Secret);

           
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(signingKey),
                ValidateIssuer =false,
                ValidIssuer = "http://localhost:7000",
                ValidateAudience = false,            
                
            };

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = authenticationProviderKey;
                o.DefaultScheme= authenticationProviderKey;
            })
            .AddJwtBearer(authenticationProviderKey, x =>
            {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = tokenValidationParameters;
            });



            // Event bus settings
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>           {


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

            //var rabbitMQPersistentConnection = ser.GetRequiredService<IRabbitMQPersistentConnection>();
            
            services.AddHostedService<NewOrderCreatedEventHandler>();
            services.AddOptions();
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

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductServiceAPI, ProductServiceAPI>();
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

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //   RegisterWithConsul(app);
            var consulConfig = new ServiceDiscoveryConfiguration();
            Configuration.GetSection("consulConfig").Bind(consulConfig);
            app.UseConsul(Configuration);
        }

        private static void RegisterWithConsul(IApplicationBuilder app, IOptions<ServiceDiscoveryConfiguration> options)
        {
            // Retrieve Consul client from DI
            var consulClient = app.ApplicationServices
                                .GetRequiredService<IConsulClient>();


            // Register service with consul
            var uri = new Uri("http://localhost");
            var registration = new AgentServiceRegistration()
            {
                ID = "productService", //$"{consulConfig.Value.ServiceID}-{uri.Port}",
                Name = "productService",//consulConfig.Value.ServiceName,
                Address = "localhost",
                Port = 7002,//uri.Port,
                Tags = new[] { "product" }
            };

            // logger.LogInformation("Registering with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();


        }
    }
}
