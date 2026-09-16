using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransmarRecruitment.Data;
using TransmarRecruitment.Models;
using TransmarRecruitment.DTOs;

namespace TransmarRecruitment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.Products.ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _db.Products.FindAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var product = new Product { Name = model.Name };

            if (model.AssemblyLines != null && model.AssemblyLines.Any())
            {
                product.AssemblyLines = model.AssemblyLines.Select(a => new AssemblyLine
                {
                    Name = a.Name,
                    Active = a.Active
                }).ToList();
            }

            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        //[Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product model)
        {
            var existing = await _db.Products.FindAsync(id);
            if (existing == null) return NotFound();
            existing.Name = model.Name;
            await _db.SaveChangesAsync();
            return Ok(existing);
        }

        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _db.Products.FindAsync(id);
            if (existing == null) return NotFound();
            _db.Products.Remove(existing);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}