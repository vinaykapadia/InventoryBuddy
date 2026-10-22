using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryBuddyDatabase.Models;

[Table(nameof(InventoryItem))]
public class InventoryItem
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string Barcode { get; set; }

    public int Quantity { get; set; }

    public virtual InventoryProduct? Product { get; set; }
    public int? ProductId { get; set; }
}
