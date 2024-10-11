using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using texasgym_backend.Models;

[Route("api/[controller]")]
[ApiController]
public class TreinosController : ControllerBase
{
    private readonly AppDbContext _context;

    public TreinosController(AppDbContext context)
    {
        _context = context;
    }

    // Obter todos os treinos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Treino>>> GetTreinos()
    {
        return await _context.Treinos.ToListAsync();
    }

    // Obter treino por ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Treino>> GetTreino(int id)
    {
        var treino = await _context.Treinos.FindAsync(id);

        if (treino == null)
        {
            return NotFound();
        }

        return treino;
    }

    // Criar novo treino
    [HttpPost]
    public async Task<ActionResult<Treino>> CriarTreino(Treino treino)
    {
        _context.Treinos.Add(treino);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTreino), new { id = treino.Id }, treino);
    }

    // Atualizar treino existente
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarTreino(int id, Treino treinoAtualizado)
    {
        if (id != treinoAtualizado.Id)
        {
            return BadRequest();
        }

        _context.Entry(treinoAtualizado).State = EntityState.Modified;

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

    // Deletar treino
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarTreino(int id)
    {
        var treino = await _context.Treinos.FindAsync(id);
        if (treino == null)
        {
            return NotFound();
        }

        _context.Treinos.Remove(treino);
        await _context.SaveChangesAsync();

        return Ok("Treino deletado com sucesso.");
    }

    private bool TreinoExists(int id)
    {
        return _context.Treinos.Any(e => e.Id == id);
    }
}
