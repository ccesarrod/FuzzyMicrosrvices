using DataCore.Entities;
using DataCore.Repository;

namespace DataCore.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {      
    }


    public class OrderRepository : Repository<Order>, IOrderRepository
    {


        public OrderRepository(CustomerOrderContext context) : base(context)
        {

        }       
    }
}
