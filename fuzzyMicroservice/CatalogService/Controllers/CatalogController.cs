using CategoryAPI;
using DataCore.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpGet]
        public ActionResult<Category> Get(int id)
        {
          return _service.GetCategoryById(id);
        }
    }
}