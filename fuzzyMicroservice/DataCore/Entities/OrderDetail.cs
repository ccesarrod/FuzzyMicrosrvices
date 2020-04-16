using System.ComponentModel.DataAnnotations;

namespace DataCore.Entities
{
    public class OrderDetail
    {
        [Key]
        public int OrderID { get; set; }
        
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public float Discount { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }

}
