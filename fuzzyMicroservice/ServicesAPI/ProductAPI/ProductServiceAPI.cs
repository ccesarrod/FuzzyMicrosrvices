using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCore.Entities;
using DataCore.Repository;
using EventCore;
using Microsoft.EntityFrameworkCore;

namespace ServicesAPI.ProductAPI
{
    public class ProductServiceAPI : IProductServiceAPI
    {
        private readonly IProductRepository _repository;

        public ProductServiceAPI(IProductRepository repository)
        {
            this._repository = repository;
        }
        public async Task<ICollection<Product>> GetAll()
        {
            return await _repository.GetAll().ToListAsync(); ;
        }

        public void Publish(IntegrationEvent integrationEvent)
        {
            throw new NotImplementedException();
        }

        public bool  UpdateQuantity(int productId, int quantity)
        {
            var updateSuccess = false;
            var product = _repository.Find(x => x.ProductID == productId).Single();
            if (product.UnitsInStock == 0) throw new OutStockException($"{product.ProductName}  is out stock with id {product.ProductID}" );
            short inventory =(short) (product.UnitsInStock.Value - quantity);
            if ( inventory < product.ReorderLevel)
            {
                product.UnitsInStock = inventory;
                _repository.Update(product);
                _repository.Save();
                updateSuccess = true;
            }

            return updateSuccess;
        }
    }
}
