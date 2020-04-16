using System.Collections.Generic;
using System.Linq;
using DataCore.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.API;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServiceAPI _service;
      
        public ProductController(IProductServiceAPI service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<Product>> Get()
        {
            return _service.GetAll().ToList();
        }
    }
}