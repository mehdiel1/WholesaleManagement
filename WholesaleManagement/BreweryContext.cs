using Microsoft.EntityFrameworkCore;
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
            modelBuilder.Entity<WholesalerStock>()
                        .HasKey(ws => new { ws.WholesalerId, ws.BeerId });

            modelBuilder.Entity<WholesalerStock>()
                        .HasOne(ws => ws.Wholesaler)
                        .WithMany(w => w.WholesalerStocks)
                        .HasForeignKey(ws => ws.WholesalerId);

            modelBuilder.Entity<WholesalerStock>()
                        .HasOne(ws => ws.Beer)
                        .WithMany(b => b.WholesalerStocks)
                        .HasForeignKey(ws => ws.BeerId);
        }

    }

}
