using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransmarRecruitment.Data;
using TransmarRecruitment.Models;

namespace TransmarRecruitment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkstationsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public WorkstationsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.Workstations.ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _db.Workstations.FindAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Workstation model)
        {
            _db.Workstations.Add(model);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = model.Id }, model);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Workstation model)
        {
            var existing = await _db.Workstations.FindAsync(id);
            if (existing == null) return NotFound();
            existing.ShortName = model.ShortName;
            existing.Name = model.Name;
            existing.PcName = model.PcName;
            await _db.SaveChangesAsync();
            return Ok(existing);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _db.Workstations.FindAsync(id);
            if (existing == null) return NotFound();
            _db.Workstations.Remove(existing);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
