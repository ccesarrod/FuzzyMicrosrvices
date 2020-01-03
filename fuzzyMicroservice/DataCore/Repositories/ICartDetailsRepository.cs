using DataCore.Entities;
using DataCore.Repository;

namespace DataCore.Repositories
{
    public interface ICartDetailsRepository : IRepository<CartDetails>
    {
    }

    public class CartDetailsRepository : Repository<CartDetails>, ICartDetailsRepository
    {
        public CartDetailsRepository(CustomerOrderContext contextDb) : base(contextDb)
        {
        }
    }

}
