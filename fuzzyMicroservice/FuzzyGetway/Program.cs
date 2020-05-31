using CacheManager.Core.Logging;
using Consul;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using System;
using System.Linq;
using System.Text;

namespace FuzzyGetway
{
    public class Program
    {
        readonly static string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public static void Main(string[] args)
        {
            //CreateWebHostBuilder(args).Build().Run();
            BuildWebHost(args).Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
               

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)

                .UseKestrel()
                //UseIIS()
                // .UseUrls("http://localhost:7000")
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true,
                            true)
                        .AddJsonFile("ocelot.json", false, false)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices(services =>
                {
                    services.AddOcelot().AddConsul();
                    services.AddCors(options =>
                    {
                        options.AddPolicy(name:MyAllowSpecificOrigins,
                                          builder =>
                                          {
                                              builder.WithOrigins("http://localhost:4200",
                                                                    "http://localhost:7000/api/catalog")
                                                                  .AllowAnyHeader()
                                                                  .AllowCredentials()
                                                                  .AllowAnyMethod();
                                          });
                    });
                    
                    var key = Encoding.ASCII.GetBytes("aqwe123fgtpade4r");
                    var authenticationProviderKey = "IdentityApiKey";
                    services.AddAuthentication(x =>
                    {
                        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                    })
                    .AddJwtBearer(authenticationProviderKey, x => {

                        x.RequireHttpsMetadata = false;
                        x.SaveToken = true;
                        x.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = false,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = false,
                    //Usually, this is your application base URL
                        ValidIssuer = "http://localhost:7003",
                            ValidateAudience = false,
                        };
                    });

                    services.AddAuthorization(options =>
                    {
                        options.AddPolicy(authenticationProviderKey, policy =>
                        {
                            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                            policy.RequireAuthenticatedUser();

                        });
                    });

                    //configure services discovery
                  // services.Configure<ConsulConfig>(Configuration.GetSection("consulConfig"));
                    services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
                    {
                        var address = "http://localhost";// Configuration["consulConfig:address"];
                        consulConfig.Address = new Uri(address);
                    }));
                })
                .Configure(a =>
                {
                    a.UseCors(MyAllowSpecificOrigins);
                    //a.UseCors(b => b
                    //// .AllowAnyOrigin()
                    // .AllowAnyMethod()
                    //  .AllowAnyHeader()
                    //  .AllowCredentials());

                    a.UseOcelot().Wait();
                   RegisterWithConsul(a);

                })
                .Build();
        }

        private static void RegisterWithConsul(IApplicationBuilder app)
        {
            // Retrieve Consul client from DI
            var consulClient = app.ApplicationServices
                                .GetRequiredService<IConsulClient>();
            var consulConfig = app.ApplicationServices
                                .GetRequiredService<IOptions<ConsulConfig>>();
            // Setup logger
            //var loggingFactory = app.ApplicationServices
            //                    .GetRequiredService<ILoggerFactory>();
         //   var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

            // Get server IP address
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>();
            var address = addresses.Addresses.First();

            // Register service with consul
            var uri = new Uri("http://localhost");
            var registration = new AgentServiceRegistration()
            {
                ID = "consul", //$"{consulConfig.Value.ServiceID}-{uri.Port}",
                Name = "consul",//consulConfig.Value.ServiceName,
                Address = "localhost",
                Port = 8300,//uri.Port,
                Tags = new[] { "catalog","product" }
            };

           // logger.LogInformation("Registering with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();

            //lifetime.ApplicationStopping.Register(() => {
            //    logger.LogInformation("Deregistering from Consul");
            //    consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            //});

         
        }
    }

}
