using Consul;
using DataCore;
using DataCore.Repositories;
using DataCore.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServicesAPI.CategoryAPI;
using System;

namespace CatalogService
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

          
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryServiceAPI, CategoryServiceAPI>();

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

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            

           // RegisterWithConsul(app);
        }

        private static void RegisterWithConsul(IApplicationBuilder app)
        {
            // Retrieve Consul client from DI
            var consulClient = app.ApplicationServices
                                .GetRequiredService<IConsulClient>();
           

            // Register service with consul
            var uri = new Uri("http://localhost");
            var registration = new AgentServiceRegistration()
            {
                ID = "catalogService", //$"{consulConfig.Value.ServiceID}-{uri.Port}",
                Name = "catalogService",//consulConfig.Value.ServiceName,
                Address = "localhost",
                Port = 7001,//uri.Port,
                Tags = new[] { "catalog" }
            };

            // logger.LogInformation("Registering with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();

           
        }
    }
}
