using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using texasgym_backend.Data;
using texasgym_backend.Models;

[Route("api/[controller]")]
[ApiController]
public class FichasController : ControllerBase
{
    private readonly AppDbContext _context;

    public FichasController(AppDbContext context)
    {
        _context = context;
    }

    // Obter todas as fichas
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Ficha>>> GetFichas()
    {
        return await _context.Fichas.ToListAsync();
    }

    // Obter ficha por ID
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult> GetFicha(int id)
    {
        var ficha = await _context.Fichas
            .Include(f => f.Treinos)
            .ThenInclude(t => t.TreinosExercicios)
            .ThenInclude(te => te.Exercicio)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (ficha == null)
        {
            return NotFound("Ficha não encontrada.");
        }

        return Ok(new
        {
            ficha.Id,
            ficha.DataCriacao,
            ficha.Observacao,
            Treinos = ficha.Treinos.Select(t => new
            {
                t.Id,
                t.Nome,
                t.Repeticoes,
                t.DiasTreino,
                t.PesoUsado,
                t.TempoDescanso,
                t.Observacao,
                Exercicios = t.TreinosExercicios.Select(te => new
                {
                    te.Exercicio.Id,
                    te.Exercicio.Nome,
                    te.Exercicio.Descricao,
                    te.Exercicio.LinkYoutube,
                    te.Repeticoes,
                    te.Peso,
                    te.TempoDescanso,
                    te.Observacao
                })
            })
        });
    }


    // Obter fichas por ID do usuário
    [HttpGet("usuario/{userId}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Ficha>>> GetFichasPorUsuario(int userId)
    {
        var fichas = await _context.Fichas
            .Where(f => f.UsuarioId == userId)
            .ToListAsync();

        if (fichas == null || fichas.Count == 0)
        {
            return NotFound("Nenhuma ficha encontrada para o usuário.");
        }

        return Ok(fichas);
    }


    // Criar nova ficha
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CriarFicha(Ficha ficha)
    {
        // Verificar se o usuário existe
        var usuario = await _context.Usuarios.FindAsync(ficha.UsuarioId);
        if (usuario == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        _context.Fichas.Add(ficha);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFicha), new { id = ficha.Id }, ficha);
    }

    // Atualizar ficha existente
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> AtualizarFicha(int id, Ficha fichaAtualizada)
    {
        if (id != fichaAtualizada.Id)
        {
            return BadRequest();
        }

        _context.Entry(fichaAtualizada).State = EntityState.Modified;

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

    // Deletar ficha
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeletarFicha(int id)
    {
        var ficha = await _context.Fichas.FindAsync(id);
        if (ficha == null)
        {
            return NotFound();
        }

        _context.Fichas.Remove(ficha);
        await _context.SaveChangesAsync();

        return Ok("Ficha deletada com sucesso.");
    }

    private bool FichaExists(int id)
    {
        return _context.Fichas.Any(e => e.Id == id);
    }
}
