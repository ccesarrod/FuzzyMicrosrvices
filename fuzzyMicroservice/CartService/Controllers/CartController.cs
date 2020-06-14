using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Consul;
using DataCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        
        private readonly IConsulClient _consulClient;
        private readonly IHttpClientFactory _factory;

        public CartController(IConsulClient consulClient, IHttpClientFactory httpClient)
        {          
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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.PostAsync(customerServiceUrl, byteContent);
              
                if (response.IsSuccessStatusCode) {
                    return Ok(new
                    {
                        cart = cartView
                    });
                }
                else {
                    return StatusCode(StatusCodes.Status500InternalServerError,response.Content);
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

        

        //private Customer GetAutenticatedCustomer()
        //{

        //    return User.Identity.IsAuthenticated ? GetCustomerByEmail() : new Customer();
        //}

        //private Customer GetCustomerByEmail()
        //{
        //    var user = HttpContext.User.Identity.Name;

        //    return _customerService.getByEmail(user);
        //}
    }
}