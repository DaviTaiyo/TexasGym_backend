using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using texasgym_backend.Models;

[Route("api/[controller]")]
[ApiController]
public class TreinosFichasController : ControllerBase
{
    private readonly AppDbContext _context;

    public TreinosFichasController(AppDbContext context)
    {
        _context = context;
    }

    // Obter todas as associações de treinos com fichas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TreinoFicha>>> GetTreinosFichas()
    {
        return await _context.TreinosFichas.ToListAsync();
    }

    // Obter associação de treino com ficha por ID
    [HttpGet("{id}")]
    public async Task<ActionResult<TreinoFicha>> GetTreinoFicha(int id)
    {
        var treinoFicha = await _context.TreinosFichas.FindAsync(id);

        if (treinoFicha == null)
        {
            return NotFound();
        }

        return treinoFicha;
    }

    // Criar nova associação entre treino e ficha
    [HttpPost]
    public async Task<ActionResult<TreinoFicha>> CriarTreinoFicha(TreinoFicha treinoFicha)
    {
        // Verificar se a ficha e o treino existem
        var ficha = await _context.Fichas.FindAsync(treinoFicha.FichaId);
        var treino = await _context.Treinos.FindAsync(treinoFicha.TreinoId);
        if (ficha == null || treino == null)
        {
            return NotFound("Ficha ou Treino não encontrado.");
        }

        _context.TreinosFichas.Add(treinoFicha);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTreinoFicha), new { id = treinoFicha.Id }, treinoFicha);
    }

    // Atualizar associação existente
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarTreinoFicha(int id, TreinoFicha treinoFichaAtualizado)
    {
        if (id != treinoFichaAtualizado.Id)
        {
            return BadRequest();
        }

        _context.Entry(treinoFichaAtualizado).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TreinoFichaExists(id))
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

    // Deletar associação
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarTreinoFicha(int id)
    {
        var treinoFicha = await _context.TreinosFichas.FindAsync(id);
        if (treinoFicha == null)
        {
            return NotFound();
        }

        _context.TreinosFichas.Remove(treinoFicha);
        await _context.SaveChangesAsync();

        return Ok("Associação entre treino e ficha deletada com sucesso.");
    }

    private bool TreinoFichaExists(int id)
    {
        return _context.TreinosFichas.Any(e => e.Id == id);
    }
}
