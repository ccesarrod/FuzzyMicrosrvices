using DataCore.Entities;
using System.Collections.Generic;

namespace ServicesAPI.ProductAPI
{
    public  interface IProductServiceAPI
    {
       IEnumerable<Product> GetAll();
    }
}
