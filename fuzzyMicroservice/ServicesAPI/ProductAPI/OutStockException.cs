using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesAPI.ProductAPI
{
    public class OutStockException: Exception
    {
        public OutStockException(string ProductName):base($"{ProductName} is out stock")
        {
            
        }
    }
}
