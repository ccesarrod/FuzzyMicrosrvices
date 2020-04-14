using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCore.Entities;
using DataCore.Repositories;

namespace OrderService.API
{
    public class OrderAPI: IOrderAPI
    {
        private readonly IOrderRepository _orderRepository;

        public OrderAPI(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public Order AddOrder(Order order)
        {
            _orderRepository.Add(order);
            _orderRepository.Save();
            return order;
        }

        public Order GetById(int id)
        {
            return _orderRepository.Find(x => x.OrderID == id).SingleOrDefault();
        }

        public List<OrderDetail> GetOrderDetails(string OrderId)
        {
            throw new NotImplementedException();
        }
    }
}
