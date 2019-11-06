using System.Collections.Generic;
using DataCore.Entities;

namespace CategoryAPI
{
    public interface ICategoryServiceAPI
    {
        List<Category> GetAll();
        Category GetCategoryById(int categoryId);
    }
}