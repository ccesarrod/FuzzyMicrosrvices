using System.Collections.Generic;
using AuthenticationService.API;
using DataCore.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.API;
using OrderService.Models;

namespace OrderServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderAPI _orderAPI;
     
        public OrderController(IOrderAPI orderAPI)
        {
            _orderAPI = orderAPI;
           
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
      
        public ActionResult<Order> Get(int id)
        {
            var order= _orderAPI.GetById(id);

            if (order == null)
            {
                return NotFound(); // Returns a NotFoundResult
            }
            return Ok(order);
        }

        // POST api/values
        [HttpPost]
        [Authorize]
        public ActionResult<OrderViewModel> Post([FromBody] OrderViewModel value)
        {
            
                var order = _orderAPI.AddOrder(value, User.Identity.Name);
            //var order = value;
            return Ok(order);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        
    }
}
