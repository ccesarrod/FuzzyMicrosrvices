using DataCore;
using DataCore.Repositories;
using DataCore.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServicesAPI.CustomerAPI;

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
            services.AddScoped<ICustomerService, ServicesAPI.CustomerAPI.CustomerService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartDetailsRepository, CartDetailsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
