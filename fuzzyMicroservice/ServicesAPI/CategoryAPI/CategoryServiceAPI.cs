using DataCore.Entities;
using DataCore.Repositories;
using DataCore.Repository;
using System.Collections.Generic;
using System.Linq;

namespace ServicesAPI.CategoryAPI
{
    public class CategoryServiceAPI : ICategoryServiceAPI
    {
        private ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public  CategoryServiceAPI(ICategoryRepository repository,IProductRepository productRepository)
        {
            _categoryRepository = repository;
            _productRepository = productRepository;
        }

    

        public List<Category> GetAll()
        {
            return _categoryRepository.GetAll().ToList();
        }

        public Category GetCategoryById(int categoryId)
        {
            return _categoryRepository.Find(x => x.CategoryID == categoryId).FirstOrDefault();
        }

        public List<Product> ProductsByCategoryId(int categoryId)
        {
           return _productRepository.Find(p => p.CategoryID == categoryId).ToList();
        }
    }

}
