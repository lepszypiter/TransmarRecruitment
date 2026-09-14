using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TransmarRecruitment.Data;
using TransmarRecruitment.Models;

namespace TransmarRecruitment.Controllers
{
    [ApiController]
    [Route("api/assembly-lines/{assemblyLineId}/workstations")]
    public class AllocationsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public AllocationsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int assemblyLineId)
        {
            var list = await _db.AssemblyLineWorkstations
                .Where(a => a.AssemblyLineId == assemblyLineId)
                .Include(a => a.Workstation)
                .OrderBy(a => a.Position)
                .ToListAsync();
            return Ok(list);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(int assemblyLineId, [FromBody] AddModel model)
        {
            var assembly = await _db.AssemblyLines.FindAsync(assemblyLineId);
            if (assembly == null) return NotFound(new { message = "Assembly line not found" });

            var currentMax = await _db.AssemblyLineWorkstations
                .Where(a => a.AssemblyLineId == assemblyLineId)
                .OrderByDescending(a => a.Position)
                .Select(a => a.Position)
                .FirstOrDefaultAsync();

            var pos = currentMax;
            if (pos == 0) pos = 0; // start at 1 below

            var created = new List<AssemblyLineWorkstation>();
            foreach (var wid in model.WorkstationIds)
            {
                pos++;
                var item = new AssemblyLineWorkstation { AssemblyLineId = assemblyLineId, WorkstationId = wid, Position = pos };
                _db.AssemblyLineWorkstations.Add(item);
                created.Add(item);
            }

            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { assemblyLineId }, created);
        }

        [Authorize]
        [HttpPut("reorder")]
        public async Task<IActionResult> Reorder(int assemblyLineId, [FromBody] ReorderModel model)
        {
            // model.Order contains allocation ids in new order
            var orderList = model.Order.ToList();
            var allocations = await _db.AssemblyLineWorkstations.Where(a => orderList.Contains(a.Id)).ToListAsync();
            var idToAlloc = allocations.ToDictionary(a => a.Id);
            for (int i = 0; i < orderList.Count; i++)
            {
                var id = orderList[i];
                if (idToAlloc.TryGetValue(id, out var alloc)) alloc.Position = i + 1;
            }
            await _db.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpDelete("{allocationId}")]
        public async Task<IActionResult> Delete(int assemblyLineId, int allocationId)
        {
            var alloc = await _db.AssemblyLineWorkstations.FindAsync(allocationId);
            if (alloc == null) return NotFound();
            _db.AssemblyLineWorkstations.Remove(alloc);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        public record AddModel(int[] WorkstationIds);
        public record ReorderModel(int[] Order);
    }
}
