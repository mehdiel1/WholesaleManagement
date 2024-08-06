namespace WholesaleManagement.Models
{
    public class Brewery
    {
        public int BreweryId { get; set; }
        public string Name { get; set; }
        public ICollection<Beer> Beers { get; set; }
    }

    public class Beer
    {
        public int BeerId { get; set; }
        public string Name { get; set; }
        public double AlcoholContent { get; set; }
        public decimal Price { get; set; }
        public int BreweryId { get; set; }
        public Brewery Brewery { get; set; }
        public ICollection<WholesalerStock> WholesalerStocks { get; set; }
    }

    public class Wholesaler
    {
        public int WholesalerId { get; set; }
        public string Name { get; set; }
        public ICollection<WholesalerStock> WholesalerStocks { get; set; }
    }

    public class WholesalerStock
    {
        public int WholesalerStockId { get; set; }
        public int WholesalerId { get; set; }
        public Wholesaler Wholesaler { get; set; }
        public int BeerId { get; set; }
        public Beer Beer { get; set; }
        public int Quantity { get; set; }
    }

}
