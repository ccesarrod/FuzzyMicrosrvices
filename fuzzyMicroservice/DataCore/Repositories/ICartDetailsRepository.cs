using DataCore.Entities;
using DataCore.Repository;
using System.Collections.Generic;

namespace DataCore.Repositories
{
    public interface ICartDetailsRepository : IRepository<CartDetails>
    {
        void DeleteRage(ICollection<CartDetails> cartDetails);
    }

    public class CartDetailsRepository : Repository<CartDetails>, ICartDetailsRepository
    {
        private readonly CustomerOrderContext _contextDb;

        public CartDetailsRepository(CustomerOrderContext contextDb) : base(contextDb)
        {
            _contextDb = contextDb;
        }

        public void DeleteRage(ICollection<CartDetails> cartDetails)
        {
            _contextDb.RemoveRange(cartDetails);
        }
    }

}
