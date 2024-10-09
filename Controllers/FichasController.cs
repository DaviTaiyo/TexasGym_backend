using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using texasgym_backend.Models;
using Microsoft.EntityFrameworkCore;
using texasgym_backend.Data;

namespace texasgym_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FichasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FichasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Fichas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fichas>>> GetFichas()
        {
            return await _context.Fichas.ToListAsync();
        }

        // GET: api/Fichas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fichas>> GetFicha(int id)
        {
            var ficha = await _context.Fichas.FindAsync(id);

            if (ficha == null)
            {
                return NotFound();
            }

            return ficha;
        }

        // POST: api/Fichas
        [HttpPost]
        public async Task<ActionResult<Fichas>> PostFicha(Fichas ficha)
        {
            _context.Fichas.Add(ficha);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFicha), new { id = ficha.Id }, ficha);
        }

        // PUT: api/Fichas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFicha(int id, Fichas ficha)
        {
            if (id != ficha.Id)
            {
                return BadRequest();
            }

            _context.Entry(ficha).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FichaExists(id))
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

        // DELETE: api/Fichas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFicha(int id)
        {
            var ficha = await _context.Fichas.FindAsync(id);
            if (ficha == null)
            {
                return NotFound();
            }

            _context.Fichas.Remove(ficha);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FichaExists(int id)
        {
            return _context.Fichas.Any(e => e.Id == id);
        }
    }
}
