using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataCore.Entities
{
    public class Product
    {

        public int ProductID { get; set; }
        [Required]
        [StringLength(10)]
        public string ProductName { get; set; }
        public int? CategoryID { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }


    }
}
