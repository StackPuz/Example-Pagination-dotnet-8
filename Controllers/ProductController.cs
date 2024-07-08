using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using App.Models;

namespace App.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("api/products")]
        public async Task<IActionResult> Index()
        {
            int page = Request.Query["page"].Any() ? Convert.ToInt32(Request.Query["page"]) : 1;
            int size = Request.Query["size"].Any() ? Convert.ToInt32(Request.Query["size"]) : 10;
            string order = Request.Query["order"].Any() ? Request.Query["order"].First() : "Id";
            string direction = Request.Query["direction"].Any() ? Request.Query["direction"].First() : "asc";
            var query = _context.Product.Select(e => new {
                Id = e.Id,
                Name = e.Name,
                Price = e.Price
            });
            query = query.OrderBy(order, direction);
            var products = await query.Skip((page - 1) * size).Take(size).ToListAsync();
            return Ok(products);
        }
    }
}
