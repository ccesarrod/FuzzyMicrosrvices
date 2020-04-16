using DataCore.Entities;
using OrderService.Models;
using System.Collections.Generic;

namespace OrderService.API
{
    public interface IOrderAPI
    {

        Order GetById(int id);

        OrderViewModel AddOrder(OrderViewModel order, string customerEmail);


        List<OrderDetail> GetOrderDetails(string OrderId);
    }
}
