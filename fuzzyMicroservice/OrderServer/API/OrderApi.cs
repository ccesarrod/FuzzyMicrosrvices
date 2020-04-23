//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//using DataCore.Entities;
//using DataCore.Repositories;
//using OrderService.Models;
//using ServicesAPI.CustomerAPI;

//namespace OrderService.API
//{
//    public class OrderAPI: IOrderAPI
//    {
//        private readonly IOrderRepository _orderRepository;
//        private readonly ICustomerService _customerService;

//        public OrderAPI(IOrderRepository orderRepository, ICustomerService customerService)
//        {
//            _orderRepository = orderRepository;
//            this._customerService = customerService;
//        }
//        public OrderViewModel AddOrder(OrderViewModel orderModel, string customerEmail)
//        {
//            var customer = _customerService.getByEmail(customerEmail);
//            Order order = MapToOrder(orderModel);
//            order.CustomerID = customer.CustomerID;
//            order.Customer = customer;

//            _orderRepository.Add(order);
//            _orderRepository.Save();
//            orderModel.Id = order.OrderID;
//            return orderModel;
//        }

//        public Order GetById(int id)
//        {
//            return _orderRepository.Find(x => x.OrderID == id).SingleOrDefault();
//        }

//        public List<OrderDetail> GetOrderDetails(string OrderId)
//        {
//            throw new NotImplementedException();
//        }

//        private Order MapToOrder(OrderViewModel orderModel)
//        {
//            return new Order
//            {
//                ShipAddress = orderModel.ShipAddress,
//                ShipCity = orderModel.ShipCity,
//                ShipCountry = orderModel.ShipCountry,
//                ShipName = orderModel.ShipName,
//                ShipPostalCode = orderModel.ShipPostalCode,
//                ShipRegion = orderModel.ShipRegion,
//                Order_Details = MapToOrderDetail(orderModel.Order_Detail)
//            };
//        }

//        private ICollection<OrderDetail> MapToOrderDetail(OrderDetailView[] order_Detail)
//        {
//            var list = new List<OrderDetail>();

//         order_Detail.ToList().ForEach(x => {

//               list.Add(new OrderDetail {ProductID= x.id, Quantity=x.quantity,UnitPrice = x.price });
//               });

//            return list;
//        }


       
//    }
//}
