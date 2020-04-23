namespace OrderService.Models
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
            Order_Detail = new OrderDetailView[] { };
        }
        public int Id { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }

        public OrderDetailView[] Order_Detail { get; set; }
    }
}
