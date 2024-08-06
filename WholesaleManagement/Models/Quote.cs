namespace WholesaleManagement.Models
{
    public class QuoteRequest
    {
        public int WholesalerId { get; set; }
        public Dictionary<int, int> BeerOrders { get; set; } // Key: BeerId, Value: Quantity
    }

    public class QuoteResponse
    {
        public decimal TotalPrice { get; set; }
        public string Summary { get; set; }
    }

}
