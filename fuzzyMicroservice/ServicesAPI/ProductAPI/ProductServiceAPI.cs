using System.Collections.Generic;
using System.Linq;
using DataCore.Entities;
using DataCore.Repository;

namespace ServicesAPI.ProductAPI
{
    public class ProductServiceAPI : IProductServiceAPI
    {
        private readonly IProductRepository _repository;

        public ProductServiceAPI(IProductRepository repository)
        {
            this._repository = repository;
        }
        public IEnumerable<Product> GetAll()
        {
            return _repository.GetAll();
        }

        public bool  UpdateQuantity(int productId, int quantity)
        {
            var updateSuccess = false;
            var product = _repository.Find(x => x.ProductID == productId).Single();
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
