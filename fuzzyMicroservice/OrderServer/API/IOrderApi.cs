using DataCore.Entities;
using System.Collections.Generic;

namespace OrderService.API
{
    public interface IOrderAPI
    {

        Order GetById(int id);

        Order AddOrder(Order order);


        List<OrderDetail> GetOrderDetails(string OrderId);
    }
}
