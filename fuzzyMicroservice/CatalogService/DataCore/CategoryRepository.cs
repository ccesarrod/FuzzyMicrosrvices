using DataCore;
using DataCore.Entities;

namespace CatalogService.DataCore
{
    public interface ICategoryRepository : IRepository<Category>
    {
    }

    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(CustomerOrderContext contextDb) : base(contextDb)
        {
        }
    }
}
