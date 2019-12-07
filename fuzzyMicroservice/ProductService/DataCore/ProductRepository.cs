using DataCore;
using DataCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.DataCore
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(CustomerOrderContext context) : base(context)
        {
        }


    }
}
