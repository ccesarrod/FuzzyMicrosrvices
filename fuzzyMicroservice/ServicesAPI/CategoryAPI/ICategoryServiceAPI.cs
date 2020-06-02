using System.Collections.Generic;
using System.Threading.Tasks;
using DataCore.Entities;

namespace ServicesAPI.CategoryAPI

{
    public interface ICategoryServiceAPI
    {
        Task<ICollection<Category>> GetAll();
        Category GetCategoryById(int categoryId);
        ICollection<Product> ProductsByCategoryId(int categoryId);
    }
}