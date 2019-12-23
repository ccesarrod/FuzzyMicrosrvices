using DataCore.Entities;

namespace DataCore.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(CustomerOrderContext context) : base(context)
        {
        }


    }
}
