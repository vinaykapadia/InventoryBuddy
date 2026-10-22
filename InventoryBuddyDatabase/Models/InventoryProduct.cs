using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryBuddyDatabase.Models;

[Table(nameof(InventoryProduct))]
public class InventoryProduct
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual InventoryCategory Category { get; set; }
    public int CategoryId { get; set; }

    public virtual List<InventoryItem> Items { get; set; }
}
