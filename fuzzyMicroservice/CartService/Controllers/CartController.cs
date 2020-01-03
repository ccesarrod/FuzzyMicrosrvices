using System.Collections.Generic;
using System.Linq;
using AuthenticationService.API;
using DataCore.Entities;
using DataCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CartController(ICustomerService customerAPI)
        {
            _customerService = customerAPI;
        }

        [HttpPost]
        [Authorize]
        public ActionResult Save(Cart[] cartView)
        {
            if (HttpContext.User.Identities.Any())
            {
                var customer = GetAutenticatedCustomer();
                _customerService.SyncShoppingCart(customer.Email, cartView.ToList());

                return Ok(new
                {
                    cart = cartView
                });
            }

            return NotFound();
        }

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        private Customer GetAutenticatedCustomer()
        {

            return User.Identity.IsAuthenticated ? GetCustomerByEmail() : new Customer();
        }

        private Customer GetCustomerByEmail()
        {
            var user = HttpContext.User.Identity.Name;

            return _customerService.getByEmail(user);
        }
    }
}