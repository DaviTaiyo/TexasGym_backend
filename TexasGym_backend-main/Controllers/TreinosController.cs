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
    public class TreinosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TreinosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Treinos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Treinos>>> GetTreinos()
        {
            return await _context.Treinos.ToListAsync();
        }

        // GET: api/Treinos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Treinos>> GetTreino(int id)
        {
            var treino = await _context.Treinos.FindAsync(id);

            if (treino == null)
            {
                return NotFound();
            }

            return treino;
        }

        // POST: api/Treinos
        [HttpPost]
        public async Task<ActionResult<Treinos>> PostTreino(Treinos treino)
        {
            _context.Treinos.Add(treino);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTreino), new { id = treino.Id }, treino);
        }

        // PUT: api/Treinos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTreino(int id, Treinos treino)
        {
            if (id != treino.Id)
            {
                return BadRequest();
            }

            _context.Entry(treino).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TreinoExists(id))
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

        // DELETE: api/Treinos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTreino(int id)
        {
            var treino = await _context.Treinos.FindAsync(id);
            if (treino == null)
            {
                return NotFound();
            }

            _context.Treinos.Remove(treino);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TreinoExists(int id)
        {
            return _context.Treinos.Any(e => e.Id == id);
        }
    }
}
