using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexttech.Data;
using Nexttech.Models;

namespace Nexttech.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        // Constructor that injects the DatabaseContext
        public MaterialsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/materials/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Material>>> GetMaterials()
        {
            var materials = await _context.Materials.ToListAsync();
            return Ok(materials);
        }

        // Temp material creator   

        [HttpPost("temp")]
        public async Task<IActionResult> CreateTemporaryMaterial([FromBody] TempMaterialDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || dto.MaterialCost <= 0 || dto.MaterialDensity <= 0)
                return BadRequest("Invalid material data.");

            var material = new Material
            {
                Name = dto.Name,
                Material_cost = dto.MaterialCost,
                Material_density = dto.MaterialDensity,
                IsTemporary = true
            };

            _context.Materials.Add(material);
            await _context.SaveChangesAsync();

            return Ok(new { material.Id });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Material>> GetMaterial(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }

            return Ok(material);
        }

        // POST: api/materials
        [HttpPost]
        public async Task<ActionResult<Material>> CreateMaterial([FromBody] Material material)
        {
            if (material == null)
                return BadRequest("Invalid material data.");

            _context.Materials.Add(material);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMaterial), new { id = material.Id }, material);
        }

        // PUT: api/materials/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaterial(int id, Material material)
        {
            if (id != material.Id)
            {
                return BadRequest();
            }

            _context.Entry(material).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/materials/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null || !material.IsTemporary)
            {
                return NotFound();
            }

            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MaterialExists(int id)
        {
            return _context.Materials.Any(e => e.Id == id);
        }
    }
}

