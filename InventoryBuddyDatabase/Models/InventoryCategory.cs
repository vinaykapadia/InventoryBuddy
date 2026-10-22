using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryBuddyDatabase.Models;

[Table(nameof(InventoryCategory))]
public class InventoryCategory
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual List<InventoryProduct> Products { get; set; }
}
