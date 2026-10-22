using InventoryBuddyDatabase;
using InventoryBuddyDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryBuddyService;

public class DatabaseAccess(InventoryContext db)
{
    public IEnumerable<InventoryCategory> GetCategories()
    {
        return db.InventoryCategories
            .Include(category => category.Products)
            .ThenInclude(product => product.Items);
    }

    public IEnumerable<InventoryItem> GetUncategorized()
    {
        return db.InventoryItems.Where(i => !i.ProductId.HasValue);
    }

    public void AddTransaction(string barcode, int qty)
    {
        using var scope = db.Database.BeginTransaction();
        db.Database.ExecuteSqlRaw("SELECT TOP 1 Barcode FROM InventoryItem WITH (TABLOCKX, HOLDLOCK)");

        var item = db.InventoryItems.FirstOrDefault(i => i.Barcode == barcode);

        if (item == null)
        {
            item = new InventoryItem { Barcode = barcode };
            db.InventoryItems.Add(item);
        }

        item.Quantity += qty;
        db.SaveChanges();

        scope.Commit();
    }
}
