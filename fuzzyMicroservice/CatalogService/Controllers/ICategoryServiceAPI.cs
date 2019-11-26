﻿using System.Collections.Generic;
using DataCore.Entities;

namespace CatalogService
{
    public interface ICategoryServiceAPI
    {
        List<Category> GetAll();
        Category GetCategoryById(int categoryId);
    }
}