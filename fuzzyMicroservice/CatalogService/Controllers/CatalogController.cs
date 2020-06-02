using Consul;
using DataCore.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAPI.CategoryAPI;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Controllers
{
    [ApiController]

    [Route("/api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ICategoryServiceAPI _service;
        private readonly IConsulClient _consulClient;

        public CatalogController(ICategoryServiceAPI service, IConsulClient consulClient)
        {
            _service = service;
            _consulClient = consulClient;
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<ActionResult> All()
        {
            var services = _consulClient.Agent.Services().Result.Response;
            // var services = await _consulClient.KV.Get("product");
            //var test = Encoding.UTF8.GetString(services.Response.Value, 0,
            //    services.Response.Value.Length);
            //foreach (var service in services)
            //{
            //    Console.WriteLine(service.Key + "" + service.Value.Address);
            //}

            try
            {
               
                var catalogList = await _service.GetAll();

                return Ok(catalogList);
            }
            catch
            {
                return NotFound();
            }
            
        }

        [HttpGet("{id}")]
        
        public ActionResult<Category> Get(int id)
        {
          return _service.GetCategoryById(id);
        }

        [HttpGet("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult ProductsByCategoryId(int id)        {

            try
            {
                var results= _service.ProductsByCategoryId(id);
                return Ok(results);
            }
            catch ( Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
           

        }
    }
}