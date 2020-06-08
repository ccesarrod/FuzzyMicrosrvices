using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace ServiceDiscovery
{


    public static class ServiceDiscoveryRegistration
    {

        public static void UseConsul(this IApplicationBuilder app,IConfiguration configuration)
        {
            // Retrieve Consul client from DI
            IConsulClient consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var config = app.ApplicationServices.GetService <IOptions<ServiceDiscoveryConfiguration>>();

            // Register service with consul

            var registration = new AgentServiceRegistration()
            {
                ID = config.Value.ID,// configuration["consulConfig:ID"], // "productService", //$"{consulConfig.Value.ServiceID}-{uri.Port}",
                Name = config.Value.Name,//configuration["consulConfig:Name"],//consulConfig.Value.ServiceName,
                Address = config.Value.Address,
                Port = config.Value.Port
            };


            // logger.LogInformation("Registering with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();
        }
    }
}
