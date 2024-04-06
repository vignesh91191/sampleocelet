using Microsoft.AspNetCore.Mvc;
using TestDemo.Data;
using TestDemo.Model;
using TestDemo.Services;

namespace TestDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly InventoryDbContext _context;

        private readonly BlobStorageService _blobStorageService;
        public CurrencyController(InventoryDbContext context,
            BlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
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

        [HttpPost("upload")]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            try
            {
                var uri = await _blobStorageService.UploadFileAsync(file);
                return Ok($"File uploaded successfully. Blob URI: {uri}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("ReadData")]
        public async Task<string> ReadData(string path)
        {
            try
            {
                var data = await _blobStorageService.ReadFile(path);
                var item = new InventoryItem()
                {
                    SkuName = data.SkuName,
                    Cost = data.Cost,
                    Description = data.Description,
                    ProfitPerItem = data.ProfitPerItem,
                    Quantity = data.Quantity,
                    WasDeleted = data.WasDeleted,
                    CostCurrencyId = 1
                };

                _context.InventoryItems.Add(item);
                _context.SaveChanges(true);
                return "done";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
