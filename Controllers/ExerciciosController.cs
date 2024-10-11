using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using texasgym_backend.Models;

[Route("api/[controller]")]
[ApiController]
public class ExerciciosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ExerciciosController(AppDbContext context)
    {
        _context = context;
    }

    // Obter todos os exercícios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Exercicio>>> GetExercicios()
    {
        return await _context.Exercicios.ToListAsync();
    }

    // Obter exercício por ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Exercicio>> GetExercicio(int id)
    {
        var exercicio = await _context.Exercicios.FindAsync(id);

        if (exercicio == null)
        {
            return NotFound();
        }

        return exercicio;
    }

    // Criar novo exercício
    [HttpPost]
    public async Task<ActionResult<Exercicio>> CriarExercicio(Exercicio exercicio)
    {
        // Verificar se a ficha existe
        var ficha = await _context.Fichas.FindAsync(exercicio.FichaId);
        if (ficha == null)
        {
            return NotFound("Ficha não encontrada.");
        }

        _context.Exercicios.Add(exercicio);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetExercicio), new { id = exercicio.Id }, exercicio);
    }

    // Atualizar exercício existente
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarExercicio(int id, Exercicio exercicioAtualizado)
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

    // Deletar exercício
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarExercicio(int id)
    {
        var exercicio = await _context.Exercicios.FindAsync(id);
        if (exercicio == null)
        {
            return NotFound();
        }

        _context.Exercicios.Remove(exercicio);
        await _context.SaveChangesAsync();

        return Ok("Exercício deletado com sucesso.");
    }

    private bool ExercicioExists(int id)
    {
        return _context.Exercicios.Any(e => e.Id == id);
    }
}
