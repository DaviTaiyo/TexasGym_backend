using Microsoft.AspNetCore.Mvc;
using texasgym_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using texasgym_backend.Data;

namespace texasgym_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciciosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExerciciosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Exercicios
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Exercicio>>> GetExercicios()
        {
            var exercicios = await _context.Exercicios.ToListAsync();
            return Ok(exercicios);
        }

        // GET: api/Exercicios/ID
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Exercicio>> GetExercicio(int id)
        {
            var exercicio = await _context.Exercicios.FirstOrDefaultAsync(e => e.Id == id);

            if (exercicio == null)
            {
                return NotFound();
            }

            return exercicio;
        }

        // POST: api/Exercicios
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Exercicio>> PostExercicio(Exercicio exercicio)
        {
            _context.Exercicios.Add(exercicio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExercicio), new { id = exercicio.Id }, exercicio);
        }

        // PUT: api/Exercicios/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutExercicio(int id, Exercicio exercicioAtualizado)
        {
            if (id != exercicioAtualizado.Id)
            {
                return BadRequest();
            }

            _context.Entry(exercicioAtualizado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExercicioExists(id))
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

        // DELETE: api/Exercicios/ID
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteExercicio(int id)
        {
            var exercicio = await _context.Exercicios.FindAsync(id);
            if (exercicio == null)
            {
                return NotFound();
            }

            _context.Exercicios.Remove(exercicio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExercicioExists(int id)
        {
            return _context.Exercicios.Any(e => e.Id == id);
        }
    }
}
