using System.Collections.Generic;
using DataCore.Entities;
using DataCore.Repository;

namespace ProductService.API
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
    }
}
