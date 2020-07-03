using DataCore.Entities;
using EventCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesAPI.ProductAPI
{
    public  interface IProductServiceAPI
    {
       Task<ICollection<Product>> GetAll();
        bool UpdateQuantity (int ProductId, int quantity);

        void Publish(IntegrationEvent integrationEvent);
    }
}
