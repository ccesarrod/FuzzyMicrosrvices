using DataCore.Entities;
using Microsoft.AspNetCore.Mvc;
using ServicesAPI.CategoryAPI;
using System.Collections.Generic;

namespace CatalogService.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ICategoryServiceAPI _service;

        public CatalogController(ICategoryServiceAPI service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> All()
        {
            return _service.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Category> Get(int id)
        {
          return _service.GetCategoryById(id);
        }

        [HttpGet("[action]/{id}")]
        public IEnumerable<Product> ProductsByCategoryId(int id)        {

            return _service.ProductsByCategoryId(id);
           

        }
    }
}