using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SQLitePCL;
using System.Data;
using System.Threading;
using TransmarRecruitment.Data;
using TransmarRecruitment.DTOs;
using TransmarRecruitment.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                    Active = a.Active,
                    ProductId = a.ProductId
                }).ToList();
            }

            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        //[Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _db.Products.Include(p => p.AssemblyLines).FirstOrDefaultAsync(p => p.Id == id);
            if (existing == null) return NotFound();

            existing.Name = model.Name;

            if (model.AssemblyLines != null)
            {
                // Remove existing assembly lines and replace with provided ones
                if (existing.AssemblyLines != null && existing.AssemblyLines.Any())
                {
                    _db.AssemblyLines.RemoveRange(existing.AssemblyLines);
                }

                existing.AssemblyLines = model.AssemblyLines.Select(a => new AssemblyLine
                {
                    Name = a.Name,
                    Active = a.Active
                }).ToList();
            }

            await _db.SaveChangesAsync();
            return Ok(existing);
        }

        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _db.Products
                .Include(p => p.AssemblyLines)
                .ThenInclude(al => al.Allocations)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existing == null) return NotFound();

            // Remove dependent data to avoid FK constraint issues (works with existing DB schema)
            var assemblyLineIds = existing.AssemblyLines?.Select(al => al.Id).ToArray();

            if (assemblyLineIds != null && assemblyLineIds.Length > 0)
            {
                // Remove allocations referencing these assembly lines
                var allocations = _db.AssemblyLineWorkstations.Where(a => assemblyLineIds.Contains(a.AssemblyLineId));
                _db.AssemblyLineWorkstations.RemoveRange(allocations);

                // Remove assembly lines
                var lines = _db.AssemblyLines.Where(al => assemblyLineIds.Contains(al.Id));
                _db.AssemblyLines.RemoveRange(lines);
            }

            // Remove product
            _db.Products.Remove(existing);

            // Save all changes in a transaction
            using (var tx = await _db.Database.BeginTransactionAsync())
            {
                await _db.SaveChangesAsync();
                await tx.CommitAsync();
            }

            return NoContent();
        }
    }
}