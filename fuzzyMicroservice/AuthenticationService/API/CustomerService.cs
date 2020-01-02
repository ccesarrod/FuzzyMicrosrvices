﻿using System;
using System.Collections.Generic;
using System.Linq;
using DataCore.Entities;
using DataCore.Models;
using DataCore.Repositories;
using DataCore.Repository;

namespace AuthenticationService.API
{
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public CustomerService(ICustomerRepository context, IProductRepository product)
        {
            _customerRepository = context;
            _productRepository = product;
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

            var user = _customerRepository.GetAll().SingleOrDefault(x => x.Email == email);


            if (user == null)
                return null;


            return user;
        }

        public Customer getByEmail(string email)
        {
            return _customerRepository.Find(x => x.Email == email).SingleOrDefault();
        }

        public Customer GetById(string id)
        {
            return _customerRepository.Find(x => x.CustomerID == id).SingleOrDefault();
        }

       

       

        private Product GetProductById(int id)
        {
            return _productRepository.Find(x => x.ProductID == id).SingleOrDefault();
        }

        public List<CartDetails> SyncShoppingCart(string userEmail, List<Cart> cartUpdates)
        {
            var customer = getByEmail(userEmail);


            if (customer == null || !cartUpdates.Any()) return customer.Cart;

            if (customer != null && customer.Cart == null)
            {
                customer.Cart = cartUpdates.Select(
                    x => new CartDetails { Price = x.Price, Quantity = x.Quantity, ProductId = x.Id, Product = GetProductById(x.Id) })
                    .ToList();
                _customerRepository.Update(customer);
                _customerRepository.Save();

            }
            {
                if (customer.Cart == null)
                {
                    customer.Cart = new List<CartDetails>();
                }

                //var cartList = _cartDetailsRepository.Find(x => x.CustomerID == customer.CustomerID).ToList();

                customer.Cart.Clear();
                _customerRepository.Update(customer);
                _customerRepository.Save();

                //customer.Cart.RemoveAll(x => x.CustomerID != null);

                foreach (var item in cartUpdates)
                {
                    var cartItem = customer.Cart.SingleOrDefault(x => x.ProductId == item.Id);
                    if (cartItem == null)
                        customer.Cart.Add(new CartDetails
                        {
                            Price = item.Price,
                            Quantity = item.Quantity,
                            ProductId = item.Id,
                            Product = GetProductById(item.Id)
                        });

                    else

                        if (cartItem.Quantity != item.Quantity) cartItem.Quantity += item.Quantity;
                }
            }


            _customerRepository.Update(customer);
            _customerRepository.Save();
            return customer.Cart;
        }
    }
}
