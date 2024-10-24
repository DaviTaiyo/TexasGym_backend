using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using texasgym_backend.Data;
using texasgym_backend.Models;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> GetExercicios()
        {
            var exercicios = await _context.Exercicios.ToListAsync();
            return Ok(exercicios);
        }

        // GET: api/Exercicios/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExercicio(int id)
        {
            var exercicio = await _context.Exercicios.FindAsync(id);
            if (exercicio == null)
                return NotFound();
            return Ok(exercicio);
        }

        // POST: api/Exercicios
        [HttpPost]
        public async Task<IActionResult> PostExercicio([FromBody] Exercicio exercicio)
        {
            _context.Exercicios.Add(exercicio);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetExercicio), new { id = exercicio.Id }, exercicio);
        }

        // PUT: api/Exercicios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExercicio(int id, [FromBody] Exercicio exercicio)
        {
            if (id != exercicio.Id)
                return BadRequest();

            _context.Entry(exercicio).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Exercicios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercicio(int id)
        {
            var exercicio = await _context.Exercicios.FindAsync(id);
            if (exercicio == null)
                return NotFound();

            _context.Exercicios.Remove(exercicio);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
