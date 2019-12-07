using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCore.Entities;
using Microsoft.AspNetCore.Http;
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
        public ActionResult<IEnumerable<Product>> Get()
        {
            return _service.GetAll().ToList();
        }
    }
}