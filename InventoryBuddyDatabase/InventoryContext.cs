using InventoryBuddyDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryBuddyDatabase;

public class InventoryContext : DbContext
{
    public DbSet<InventoryCategory> InventoryCategories { get; set; }

    public DbSet<InventoryProduct> InventoryProducts { get; set; }

    public DbSet<InventoryItem> InventoryItems { get; set; }

    public InventoryContext(DbContextOptions options)
    : base(options)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InventoryCategory>()
            .HasMany(category => category.Products)
            .WithOne(product => product.Category);

        modelBuilder.Entity<InventoryProduct>()
            .HasMany(product => product.Items)
            .WithOne(item => item.Product);
    }
}
