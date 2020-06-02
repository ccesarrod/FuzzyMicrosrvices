using DataCore.Entities;
using DataCore.Repositories;
using DataCore.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    

        public async Task<ICollection<Category>> GetAll()
        {
            return await _categoryRepository.GetAll().ToListAsync();
        }

        public Category GetCategoryById(int categoryId)
        {
            return _categoryRepository.Find(x => x.CategoryID == categoryId).FirstOrDefault();
        }

        public ICollection<Product> ProductsByCategoryId(int categoryId)
        {
          return  _productRepository.Find(p => p.CategoryID == categoryId).ToList();
        }

    }

}
