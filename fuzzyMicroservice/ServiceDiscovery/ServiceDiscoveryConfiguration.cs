using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceDiscovery
{
    public class ServiceDiscoveryConfiguration
    {
        //ID = "productService", //$"{consulConfig.Value.ServiceID}-{uri.Port}",
        //        Name = "productService",//consulConfig.Value.ServiceName,
        //        Address = "localhost",
        //        Port = 7002,//uri.Port,
        //        Tags = new[] { "product" }

        public string ID { get; set; }
        public  string Name { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
}
}
