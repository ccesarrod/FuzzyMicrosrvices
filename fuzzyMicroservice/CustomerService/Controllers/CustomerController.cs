using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using CustomerService.Models;
using DataCore.Entities;
using DataCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public CustomerController(ICustomerService customerAPI)
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
                try
                {
                    _customerService.SyncShoppingCart(customer, cartView.ToList());

                    return Ok(new
                    {
                        cart = cartView
                    });
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex);

                }
            }

            return NotFound();
        }
        // GET: api/<CustomerController>
        [HttpGet("getcustomerbyemail")]
        public ActionResult<Customer> GetCustomerByEmail(string email)
        {
            try
            {

                var customer = _customerService.getByEmail(email);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        // GET: api/<CustomerController>
        [HttpGet("orders")]
        [Authorize]
        public ActionResult<OrderViewModel[]> Orders()
        {
            if (HttpContext.User.Identities.Any())
            {
                try
                {
                    var email = HttpContext.User.Identity.Name;
                    var customer = _customerService.getByEmail(email);
                    var mapped_orders = MapOrdersToView(customer.Orders.OrderByDescending(x=>x.OrderDate).ToList());
                    return Ok(mapped_orders);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex);
                }
            }

            return NotFound();
        }



        [HttpDelete("deletecart")]
        [Authorize]
        public ActionResult DeleteCart()
        {
            if (HttpContext.User.Identities.Any())
            {
                try
                {
                    var user = HttpContext.User.Identity.Name;
                    var customer = _customerService.getByEmail(user);
                    _customerService.DeleteShoppingCart(customer);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex);
                }

            }

            return NotFound("customer not found");
        }


        private ICollection<OrderViewModel> MapOrdersToView(List<Order> orders)
        {
            var results = new List<OrderViewModel>();
            orders.ForEach(order =>
            {
                results.Add(new OrderViewModel
                {
                    OrderID = order.OrderID,
                    OrderDate = order.OrderDate,
                    ShipAddress = order.ShipAddress,
                    ShipCity = order.ShipCity,
                    ShipCountry = order.ShipCountry,
                    ShipName = order.ShipName,
                    ShippedDate = order.ShippedDate,
                    ShipPostalCode = order.ShipPostalCode,
                    ShipRegion = order.ShipRegion                    
                });
            });
            return results.ToArray();
        }
    }

   
}
