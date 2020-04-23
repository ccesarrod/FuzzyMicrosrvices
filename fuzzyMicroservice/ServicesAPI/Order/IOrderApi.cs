using DataCore.Entities;
using System.Collections.Generic;

namespace ServicesAPI.OrderAPI
{
    public interface IOrderAPI
    {

        Order GetById(int id);

        Order AddOrder(Order order, string customerEmail);


        List<OrderDetail> GetOrderDetails(string OrderId);
    }
}
