using CategoryAPI.Repository;
using DataCore.Entities;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class Tests
    {
        List<Category> _categoryList;
        Category _category;

        [SetUp]
        public void Setup()
        {
            _categoryList = new List<Category> {
                    new Category { CategoryID = 1, CategoryName = "watches" },
                    new Category { CategoryID = 2, CategoryName = "food" }};
        }

       
    }
}