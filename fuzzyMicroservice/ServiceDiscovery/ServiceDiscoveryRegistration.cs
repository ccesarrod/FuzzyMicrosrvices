using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions;
using Microsoft.Extensions.Configuration;

namespace ServiceDiscovery
{


    public static class ServiceDiscoveryRegistration
    {

        public static void UseConsul(this IApplicationBuilder app,IConfiguration configuration)
        {
            // Retrieve Consul client from DI
            IConsulClient consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
           
            
            // Register service with consul

            var registration = new AgentServiceRegistration()
            {
                ID = configuration["consulConfig:ID"], // "productService", //$"{consulConfig.Value.ServiceID}-{uri.Port}",
                Name = configuration["consulConfig:Name"],//consulConfig.Value.ServiceName,
                Address = configuration["consulConfig:Address"],
                Port = int.Parse(configuration["consulConfig:Port"])//uri.Port
            };


            // logger.LogInformation("Registering with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();
        }
    }
}
