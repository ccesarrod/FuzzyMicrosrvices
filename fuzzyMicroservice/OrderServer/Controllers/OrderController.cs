using System.Collections.Generic;
using System.Linq;
using DataCore.Entities;
using EventCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        public OrderController(IOrderAPI orderAPI, IEventPublisher eventPublisher)
        {
            _orderAPI = orderAPI;
            _eventPublisher = eventPublisher;
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
    }
}
