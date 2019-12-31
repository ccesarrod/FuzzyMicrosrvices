using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using DataCore.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FuzzyGetway
{
    public class Program
    {
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
                    services.AddOcelot();
                    services.AddCors();
                    // configure strongly typed settings objects

                    //  var appSettingsSection = Configuration.GetSection("AppSettings");
                    // services.Configure<AppSettings>(appSettingsSection);

                    // configure jwt authentication
                    // var appSettings = appSettingsSection.Get<AppSettings>();
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

                })
                .Configure(a =>
                {
                    a.UseCors(b => b
                      .AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials());

                    a.UseOcelot().Wait();
                })
                .Build();
        }
    }

}
