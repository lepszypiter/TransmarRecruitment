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
    public class AssemblyLinesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public AssemblyLinesController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? productId)
        {
            var query = _db.AssemblyLines.AsQueryable();
            if (productId.HasValue) query = query.Where(a => a.ProductId == productId.Value);
            var list = await query.ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _db.AssemblyLines.FindAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AssemblyLineDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var entity = new AssemblyLine { Name = model.Name, Active = model.Active, ProductId = model.ProductId };
            _db.AssemblyLines.Add(entity);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }

        //[Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AssemblyLineUpdateDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var existing = await _db.AssemblyLines.FindAsync(id);
            if (existing == null) return NotFound();
            existing.Name = model.Name;
            existing.Active = model.Active;
            existing.ProductId = model.ProductId;
            await _db.SaveChangesAsync();
            return Ok(existing);
        }

        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _db.AssemblyLines.FindAsync(id);
            if (existing == null) return NotFound();
            _db.AssemblyLines.Remove(existing);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
