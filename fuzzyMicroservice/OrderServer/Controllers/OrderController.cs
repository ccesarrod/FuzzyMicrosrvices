using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Consul;
using DataCore.Entities;
using EventCore.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Events;
using OrderService.Models;
using ServicesAPI.OrderAPI;

namespace OrderServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderAPI _orderAPI;
        private readonly IEventPublisher _eventPublisher;
        private readonly IConsulClient _consulClient;
        private readonly IHttpClientFactory _factory;

        public OrderController(IOrderAPI orderAPI, 
                            IEventPublisher eventPublisher,
                            IConsulClient consulClient, IHttpClientFactory httpClient)
        {
            _orderAPI = orderAPI;
            _eventPublisher = eventPublisher;
            _consulClient = consulClient;
            this._factory = httpClient;
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

       
        [HttpGet("getCustomerOrders")]
        [Authorize]
        public async Task<ActionResult<Order>> GetCustomerOrdersAsync()
        {
            if (HttpContext.User.Identities.Any())
            {
                var client = _factory.CreateClient();
                try
                {
                    var user = HttpContext.User.Identity.Name;
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    var customerServiceUrl = GetCustomerServiceUrl("orders");
                    var response = await client.GetAsync(customerServiceUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        return Ok();
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, response.Content);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex);
                }
            }

            return NotFound();
        }

        // POST api/values
        [HttpPost("addorder")]
        [Authorize]
        public ActionResult<OrderViewModel> AddOrder([FromBody] OrderViewModel value)
        {
            var mapToOrder = MapToOrder(value);
            Order order = _orderAPI.AddOrder(mapToOrder, User.Identity.Name);
            var order_event = new NewOrderStartedEvent() { OrderId = order.OrderID, Order_Detail = value.Order_Detail };     
            
            _eventPublisher.Publish(order_event);
            return Ok(MapToOrderViewModel(order));
        }


        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private Order MapToOrder(OrderViewModel orderModel)
        {
            return new Order
            {
                ShipAddress = orderModel.ShipAddress,
                ShipCity = orderModel.ShipCity,
                ShipCountry = orderModel.ShipCountry,
                ShipName = orderModel.ShipName,
                ShipPostalCode = orderModel.ShipPostalCode,
                ShipRegion = orderModel.ShipRegion,
                Order_Details = MapToOrderDetail(orderModel.Order_Detail)
            };
        }

        private OrderViewModel MapToOrderViewModel(Order orderModel)
        {
            return new OrderViewModel
            {
                ShipAddress = orderModel.ShipAddress,
                ShipCity = orderModel.ShipCity,
                ShipCountry = orderModel.ShipCountry,
                ShipName = orderModel.ShipName,
                ShipPostalCode = orderModel.ShipPostalCode,
                ShipRegion = orderModel.ShipRegion               
            };
        }

        private ICollection<OrderDetail> MapToOrderDetail(OrderDetailView[] order_Detail)
        {
            var list = new List<OrderDetail>();

            order_Detail.ToList().ForEach(x => {

                list.Add(new OrderDetail { ProductID = x.id, Quantity = (short)x.quantity, UnitPrice = x.price });
            });

            return list;
        }

        private string GetCustomerServiceUrl(string action)
        {
            var actionPath = "";
            switch (action)
            {
                case "save":
                    actionPath = "savecart";
                    break;
                case "delete":
                    actionPath = "deletecart";
                    break;
            }

            var services = _consulClient.Agent.Services().Result.Response;

            var service = services.FirstOrDefault(x => x.Key == "customerService");
            var url = $"http://{service.Value.Address}:7000/api/customer/{actionPath}";
            return url;
        }
    }
}
