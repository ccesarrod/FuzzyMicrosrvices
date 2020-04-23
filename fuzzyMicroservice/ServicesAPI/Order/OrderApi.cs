using System;
using System.Collections.Generic;
using System.Linq;

using DataCore.Entities;
using DataCore.Repositories;
using ServicesAPI.CustomerAPI;

namespace ServicesAPI.OrderAPI
{
    public class OrderAPI: IOrderAPI
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerService _customerService;

        public OrderAPI(IOrderRepository orderRepository, ICustomerService customerService)
        {
            _orderRepository = orderRepository;
            this._customerService = customerService;
        }
        public Order AddOrder(Order order, string customerEmail)
        {
            var customer = _customerService.getByEmail(customerEmail);          
            order.CustomerID = customer.CustomerID;
            order.Customer = customer;

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
