using System.Collections.Generic;
using System.Linq;
using DataCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAPI.CustomerAPI;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CustomerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        CustomerController(ICustomerService customerAPI)
        {
            _customerService = customerAPI;
        }

        [HttpPost("savecart")]
        [Authorize]
        public ActionResult Save(Cart[] cartView)
        {
            if (HttpContext.User.Identities.Any())
            {
                var user = HttpContext.User.Identity.Name;
                var customer = _customerService.getByEmail(user);
                _customerService.SyncShoppingCart(customer.Email, cartView.ToList());

                return Ok(new
                {
                    cart = cartView
                });
            }

            return NotFound();
        }
        // GET: api/<CustomerController>
        [HttpGet]
        public IEnumerable<string> GetAutheti()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CustomerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
