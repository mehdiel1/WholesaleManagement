using System.Collections.Generic;
using System.Reflection.Emit;
using WholesaleManagement.Models;

namespace WholesaleManagement
{
    public class BreweryContext : DbContext
    {
        public DbSet<Brewery> Breweries { get; set; }
        public DbSet<Beer> Beers { get; set; }
        public DbSet<Wholesaler> Wholesalers { get; set; }
        public DbSet<WholesalerStock> WholesalerStocks { get; set; }

        public BreweryContext(DbContextOptions<BreweryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships and constraints here if necessary
        }
    }

}
