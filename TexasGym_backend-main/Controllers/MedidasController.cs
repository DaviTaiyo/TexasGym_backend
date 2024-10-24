using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using texasgym_backend.Data;
using texasgym_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace texasgym_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedidasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedidasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Medidas
        [HttpGet]
        public async Task<IActionResult> GetMedidas()
        {
            var medidas = await _context.Medidas.ToListAsync();
            return Ok(medidas);
        }

        // GET: api/Medidas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedida(int id)
        {
            var medida = await _context.Medidas.FindAsync(id);
            if (medida == null)
                return NotFound();
            return Ok(medida);
        }

        // POST: api/Medidas
        [HttpPost]
        public async Task<IActionResult> PostMedida([FromBody] Medida medida)
        {
            _context.Medidas.Add(medida);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMedida), new { id = medida.Id }, medida);
        }

        // PUT: api/Medidas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedida(int id, [FromBody] Medida medida)
        {
            if (id != medida.Id)
                return BadRequest();

            _context.Entry(medida).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Medidas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedida(int id)
        {
            var medida = await _context.Medidas.FindAsync(id);
            if (medida == null)
                return NotFound();

            _context.Medidas.Remove(medida);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
