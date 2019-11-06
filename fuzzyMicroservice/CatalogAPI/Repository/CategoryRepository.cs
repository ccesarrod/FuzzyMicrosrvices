using DataCore;
using DataCore.Entities;

namespace CategoryAPI.Repository
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
