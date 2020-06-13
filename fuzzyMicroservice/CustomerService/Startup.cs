using Consul;
using DataCore;
using DataCore.Entities;
using DataCore.Repositories;
using DataCore.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ServiceDiscovery;
using ServicesAPI.CustomerAPI;
using System;
using System.Text;

namespace CustomerService
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
            services.AddControllers()
                .AddNewtonsoftJson(options => {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;                    
                    });

            services.AddDbContext<CustomerOrderContext>(options => {

                //var str = @"Data Source=(localdb)\MSSQLLocalDb;Initial Catalog=WebApp.Models.MultiTenantContext;Integrated Security=True";
                options.UseLazyLoadingProxies();
                options.UseSqlServer(Configuration.GetConnectionString("Northwind"));
            });

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


            services.AddScoped<ICustomerService, ServicesAPI.CustomerAPI.CustomerService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartDetailsRepository, CartDetailsRepository>();
            services.Configure<ServiceDiscoveryConfiguration>(Configuration.GetSection("consulConfig"));

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseConsul(Configuration);
        }
       
    }
}
