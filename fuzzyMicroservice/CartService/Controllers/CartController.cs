using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Consul;
using DataCore.Entities;
using DataCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServicesAPI.CustomerAPI;

namespace CartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IConsulClient _consulClient;
        private readonly IHttpClientFactory _factory;

        public CartController(ICustomerService customerAPI, IConsulClient consulClient, IHttpClientFactory httpClient)
        {
            _customerService = customerAPI;
            _consulClient = consulClient;
            _factory = httpClient;
        }

        [HttpPost("savecart")]
        [Authorize]
        public async System.Threading.Tasks.Task<ActionResult> SaveAsync(Cart[] cartView)
        {
            var client = _factory.CreateClient();
            if (HttpContext.User.Identities.Any())
            {
                var accessToken1 = HttpContext.Request.Headers["Authorization"];
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var customerServiceUrl = GetCustomerServiceUrl();

                var myContent = JsonConvert.SerializeObject(cartView);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
             //   client.DefaultRequestHeaders.Add("Accept", "application/json");
             //   client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.PostAsync(customerServiceUrl, byteContent);
                var code =JsonConvert.SerializeObject(response.ToString());
                string result = response.Content.ReadAsStringAsync().Result;
                // var customer = GetAutenticatedCustomer();
                //  _customerService.SyncShoppingCart(customer.Email, cartView.ToList());
                if (response.IsSuccessStatusCode) {
                    return Ok(new
                    {
                        cart = cartView
                    });
                }
                else {
                    return StatusCode(500);
                }
            

            }

            return NotFound();
        }

        private string  GetCustomerServiceUrl()
        {

            var services = _consulClient.Agent.Services().Result.Response;

            var service = services.FirstOrDefault(x => x.Key == "customerService");
            var url = $"http://{service.Value.Address}:7000/api/customer/savecart";
            return url;
        }

        [HttpDelete("delete")]
        [Authorize]
        public ActionResult Delete()
        {
            if (HttpContext.User.Identities.Any())
            {
                var customer = GetAutenticatedCustomer();
                _customerService.DeleteShoppingCart(customer);
                return Ok();
            }

            return NotFound("customer not found");
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