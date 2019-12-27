using DataCore.Entities;
using DataCore.Repository;

namespace DataCore.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {

       
    }

    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
       

        public CustomerRepository(CustomerOrderContext context) : base(context)
        {

         

        }


    }
}
