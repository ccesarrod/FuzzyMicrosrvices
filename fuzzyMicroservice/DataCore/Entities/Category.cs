using System.Collections.Generic;

namespace DataCore.Entities
{
    public class Category
    {
        public Category()
        {
            Products = new List<Product>();
        }

        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
