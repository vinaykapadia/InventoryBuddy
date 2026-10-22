using InventoryBuddyService.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryBuddyService.Controllers
{
    public class HomeController(DatabaseAccess db) : Controller
    {
        public IActionResult Index()
        {
            var categories = db.GetCategories().ToList();
            var uncategorized = db.GetUncategorized().Where(i => i.Quantity != 0).ToList();
            return View(new DisplayModel
            {
                Categories = categories,
                Uncategorized = uncategorized
            });
        }
    }
}
