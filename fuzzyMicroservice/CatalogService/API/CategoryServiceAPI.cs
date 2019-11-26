using CatalogService.DataCore;
using DataCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CatalogService.API
{
    public class CategoryServiceAPI : ICategoryServiceAPI
    {
        private ICategoryRepository _repository;
        public  CategoryServiceAPI(ICategoryRepository repository)
        {
            _repository = repository;
        }

    

        public List<Category> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public Category GetCategoryById(int categoryId)
        {
            return _repository.Find(x => x.CategoryID == categoryId).FirstOrDefault();
        }
    }

}
