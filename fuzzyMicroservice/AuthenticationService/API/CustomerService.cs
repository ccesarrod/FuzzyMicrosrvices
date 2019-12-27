using System;
using System.Linq;
using DataCore.Entities;
using DataCore.Repositories;

namespace AuthenticationService.API
{
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository context)
        {
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
    }
}
