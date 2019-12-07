using DataCore;
using DataCore.Entities;

namespace ProductService.DataCore
{
    public interface IProductRepository : IRepository<Product>
    {
    }
}
