using System;
using System.Collections.Generic;
using System.Text;

namespace SpawnProcesses
{
    public class StartUpProcesses
    {
        public Boolean CatalogService{ get; set; }
        public Boolean CartService { get; set; }
        public Boolean ProductService { get; set; }
        public Boolean AuthenticationService{ get; set; }
        public Boolean OrderService { get; set; }
        public Boolean FuzzyGetway { get; set; }
        
    }
}
