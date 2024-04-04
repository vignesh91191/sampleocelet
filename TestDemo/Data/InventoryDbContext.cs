using Microsoft.EntityFrameworkCore;
using TestDemo.Model;

namespace TestDemo.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<ItemsSoldInfo> ItemSoldInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships if needed
        }
    }
}
