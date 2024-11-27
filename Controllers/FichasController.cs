using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using texasgym_backend.Data;
using texasgym_backend.Function;
using texasgym_backend.Models;

[Route("api/[controller]")]
[ApiController]
public class FichasController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly LogFunction _logFunction;

    public FichasController(AppDbContext context, LogFunction logFunction)
    {
        _context = context;
        _logFunction = logFunction;
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
        var usuario = await _context.Usuarios.FindAsync(ficha.UsuarioId);
        if (usuario == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        try
        {
            _context.Fichas.Add(ficha);
            await _context.SaveChangesAsync();

            // Log de criação da ficha
            await _logFunction.LogOperation(
                "CREATE",
                "Fichas",
                ficha.Id,
                ficha.UsuarioId,
                "SUCCESS",
                $"Ficha criada para o usuário {usuario.Nome}."
            );

            return CreatedAtAction(nameof(GetFicha), new { id = ficha.Id }, ficha);
        }
        catch (Exception ex)
        {
            await _logFunction.LogOperation(
                "CREATE",
                "Fichas",
                null,
                ficha.UsuarioId,
                "ERROR",
                $"Erro ao criar ficha: {ex.Message}"
            );

            return StatusCode(500, $"Erro ao criar ficha: {ex.Message}");
        }
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

        try
        {
            _context.Entry(fichaAtualizada).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Log de atualização da ficha
            await _logFunction.LogOperation(
                "UPDATE",
                "Fichas",
                fichaAtualizada.Id,
                fichaAtualizada.UsuarioId,
                "SUCCESS",
                $"Ficha com ID {fichaAtualizada.Id} atualizada."
            );

            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FichaExists(id))
            {
                return NotFound();
            }

            await _logFunction.LogOperation(
                "UPDATE",
                "Fichas",
                fichaAtualizada.Id,
                fichaAtualizada.UsuarioId,
                "ERROR",
                $"Erro de concorrência ao atualizar a ficha com ID {fichaAtualizada.Id}."
            );

            throw;
        }
        catch (Exception ex)
        {
            await _logFunction.LogOperation(
                "UPDATE",
                "Fichas",
                fichaAtualizada.Id,
                fichaAtualizada.UsuarioId,
                "ERROR",
                $"Erro ao atualizar ficha: {ex.Message}"
            );

            return StatusCode(500, $"Erro ao atualizar ficha: {ex.Message}");
        }
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

        try
        {
            _context.Fichas.Remove(ficha);
            await _context.SaveChangesAsync();

            // Log de exclusão da ficha
            await _logFunction.LogOperation(
                "DELETE",
                "Fichas",
                ficha.Id,
                ficha.UsuarioId,
                "SUCCESS",
                $"Ficha com ID {ficha.Id} deletada."
            );

            return Ok("Ficha deletada com sucesso.");
        }
        catch (Exception ex)
        {
            await _logFunction.LogOperation(
                "DELETE",
                "Fichas",
                ficha.Id,
                ficha.UsuarioId,
                "ERROR",
                $"Erro ao deletar ficha: {ex.Message}"
            );

            return StatusCode(500, $"Erro ao deletar ficha: {ex.Message}");
        }
    }

    private bool FichaExists(int id)
    {
        return _context.Fichas.Any(e => e.Id == id);
    }
}
