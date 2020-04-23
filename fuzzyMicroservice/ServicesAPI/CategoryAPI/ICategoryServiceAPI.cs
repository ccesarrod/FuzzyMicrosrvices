using System.Collections.Generic;
using DataCore.Entities;

namespace ServicesAPI.CategoryAPI

{
    public interface ICategoryServiceAPI
    {
        List<Category> GetAll();
        Category GetCategoryById(int categoryId);
        List<Product> ProductsByCategoryId(int categoryId);
    }
}