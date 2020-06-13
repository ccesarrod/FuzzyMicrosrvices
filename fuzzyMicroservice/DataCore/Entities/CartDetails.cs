using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataCore.Entities
{
    public class CartDetails
    {

 
        public int Id { get; set; }
        [MaxLength(5)]

        public string CustomerID { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

       // public virtual Product Product { get; set; }
       //public virtual Customer Customer { get; set; }
    }

}
