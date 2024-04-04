using Microsoft.AspNetCore.Mvc;
using TestDemo.Data;
using TestDemo.Model;

namespace TestDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly InventoryDbContext _context;

        public CurrencyController(InventoryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<InventoryItem>> GetInventoryItems()
        {
            return Ok(_context.InventoryItems);
        }

        [HttpPost]
        public ActionResult CreateInventoryItem(InventoryItem item)
        {
            _context.InventoryItems.Add(item);
            _context.SaveChanges();
            return StatusCode(201);
        }
    }
}
