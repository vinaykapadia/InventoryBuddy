using InventoryBuddyDatabase.Models;

namespace InventoryBuddyService.Models;

public class DisplayModel
{
    public List<InventoryCategory> Categories { get; set; }
    public List<InventoryItem> Uncategorized { get; set; }
}
