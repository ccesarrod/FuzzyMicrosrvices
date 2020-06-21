using System.Collections.Generic;
using System.Linq;
using DataCore.Entities;
using DataCore.Models;
using DataCore.Repositories;

namespace ServicesAPI.CustomerAPI
{
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository _customerRepository;
      
   

        public CustomerService(ICustomerRepository context) {                            
                                    
            _customerRepository = context;        
            
        }
        public Customer AddUser(Customer user)
        {
            _customerRepository.Add(user);

            return getByEmail(user.Email);
        }

        public Customer Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = getByEmail(email);


            if (user == null)
                return null;


            return user;
        }

        public Customer getByEmail(string email)
        {
           return _customerRepository.SearchFor(x => x.Email == email).SingleOrDefault();
         
        }

        public Customer GetById(string id)
        {
            return _customerRepository.Find(x => x.CustomerID == id).SingleOrDefault();
        }  

       


        public List<CartDetails> SyncShoppingCart(Customer customer, List<Cart> cartUpdates)
        {
          
          //  if (customer == null) return customer.Cart.ToList();

            if (customer != null && customer.Cart == null)
            {
                customer.Cart = cartUpdates.Select(
                    x => new CartDetails { Price = x.Price, Quantity = x.Quantity, ProductId = x.Id })
                    .ToList();
                _customerRepository.Update(customer);
                _customerRepository.Save();

            }
            {
                if (customer.Cart == null)
                {
                    customer.Cart = new List<CartDetails>();
                }

                DeleteShoppingCart(customer);
                //customer.Cart.Clear();
                //_customerRepository.Update(customer);

                //_customerRepository.Save();
               

                foreach (var item in cartUpdates)
                {
                    var cartItem = customer.Cart.SingleOrDefault(x => x.ProductId == item.Id);
                    if (cartItem == null)
                        customer.Cart.Add (new CartDetails
                        {
                            CustomerID = customer.CustomerID,                          
                            Price = item.Price,
                            Quantity = item.Quantity,
                            ProductId = item.Id                          
                        }); 
                    

                    else

                        if (cartItem.Quantity != item.Quantity) cartItem.Quantity += item.Quantity;
                }
            }


              _customerRepository.Update(customer);
        
           _customerRepository.Save();
            return customer.Cart.ToList();
        }

        public List<CartDetails> GetShoopingCart(string userEmail)
        {

            var customer = getByEmail(userEmail);
            var cart = customer.Cart.ToList();           
            return cart;
        }

        public void DeleteShoppingCart(Customer customer)
        {
           
              
                customer.Cart.Clear();
                _customerRepository.Update(customer);
                _customerRepository.Save();
       
        }

       
    }
}
