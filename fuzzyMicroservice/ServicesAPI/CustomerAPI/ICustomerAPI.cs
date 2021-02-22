using DataCore.Entities;
using DataCore.Models;
using System.Collections.Generic;

namespace ServicesAPI.CustomerAPI
{
   public interface ICustomerService
    {
        Customer Authenticate(string email, string password);
        Customer GetById(string id);

        Customer AddUser(Customer user);

        Customer getByEmail(string email);

        void DeleteShoppingCart(Customer customer);

        List<CartDetails> SyncShoppingCart(Customer customer, List<Cart> cartUpdates);
        List<CartDetails> GetShoopingCart(string userEmail);
       
    }
}
