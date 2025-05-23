using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexttech.Data;
using Nexttech.Models;

namespace Nexttech.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrintersController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public PrintersController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/printers
        [HttpGet]
        public ActionResult<IEnumerable<Printer>> GetPrinters()
        {
            var printers = _context.Printers.ToList();
            return Ok(printers);  // Should return JSON array of printers
        }

        // GET: api/printers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Printer>> GetPrinter(int id)
        {
            var printer = await _context.Printers.FindAsync(id);

            if (printer == null)
            {
                return NotFound();
            }

            return printer;
        }

        // POST: api/printers
        [HttpPost]
        public async Task<IActionResult> CreatePrinter(Printer printer)
        {
            if (printer.Id != 0)
            {
                // Optionally reject or ignore client-supplied Id here
                printer.Id = 0; // Reset to zero to insert new record
            }

            _context.Printers.Add(printer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPrinter), new { id = printer.Id }, printer);
        }


        // PUT: api/printers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrinter(int id, Printer printer)
        {
            if (id != printer.Id)
            {
                return BadRequest();
            }

            _context.Entry(printer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrinterExists(id))
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

        // DELETE: api/printers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrinter(int id)
        {
            var printer = await _context.Printers.FindAsync(id);
            if (printer == null)
            {
                return NotFound();
            }

            _context.Printers.Remove(printer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrinterExists(int id)
        {
            return _context.Printers.Any(e => e.Id == id);
        }
    }
}
