using DataCore;
using DataCore.Entities;
using DataCore.Repository;

namespace DataCore.Repositories
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
