using System.Collections.Generic;

namespace DataCore.Entities
{
    public class Customer
    {
        public Customer()
        {
            Cart = new List<CartDetails>();

        }
        public string CustomerID { get; set; }

        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

        public virtual List<CartDetails> Cart { get; set; }
    }
}
