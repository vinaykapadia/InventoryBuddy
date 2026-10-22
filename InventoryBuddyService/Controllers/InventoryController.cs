using Microsoft.AspNetCore.Mvc;

namespace InventoryBuddyService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController(DatabaseAccess db) : ControllerBase
    {
        private const string PassKey = "thisisapasskey!";

        [HttpGet("{passKey}/{barcode}/{qty:int}")]
        public IActionResult Get(string passKey, string barcode, int qty)
        {
            if (passKey == PassKey)
            {
                db.AddTransaction(barcode, qty);
                return Ok();
            }

            return BadRequest();
        }
    }
}
