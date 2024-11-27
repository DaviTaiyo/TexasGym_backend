using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using texasgym_backend.Data;
using texasgym_backend.Function;
using texasgym_backend.DTOs;
using texasgym_backend.Models;

[Route("api/[controller]")]
[ApiController]
public class TreinosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly LogFunction _logFunction;

    public TreinosController(AppDbContext context, LogFunction logFunction)
    {
        _context = context;
        _logFunction = logFunction;
    }

    // Obter todos os treinos
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Treino>>> GetTreinos()
    {
        return await _context.Treinos.ToListAsync();
    }

    // Obter treino por ID
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult> GetTreino(int id)
    {
        var treino = await _context.Treinos
            .Include(t => t.TreinosExercicios)
            .ThenInclude(te => te.Exercicio)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (treino == null)
        {
            return NotFound("Treino não encontrado.");
        }

        return Ok(new
        {
            treino.Id,
            treino.Nome,
            treino.Repeticoes,
            treino.DiasTreino,
            treino.PesoUsado,
            treino.Observacao,
            Exercicios = treino.TreinosExercicios.Select(te => new
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
        });
    }

    // Criar treino
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CriarTreino([FromBody] CriarTreinoDto request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var treino = new Treino
            {
                FichaId = request.FichaId,
                Nome = request.Nome,
                Repeticoes = request.Repeticoes,
                DiasTreino = request.DiasTreino,
                PesoUsado = request.PesoUsado,
                TempoDescanso = request.TempoDescanso,
                Observacao = request.Observacao
            };

            _context.Treinos.Add(treino);
            await _context.SaveChangesAsync();

            if (request.Exercicios != null && request.Exercicios.Any())
            {
                foreach (var exercicioId in request.Exercicios)
                {
                    var treinoExercicio = new TreinoExercicio
                    {
                        TreinoId = treino.Id,
                        ExercicioId = exercicioId,
                        Repeticoes = request.Repeticoes,
                        Peso = request.PesoUsado,
                        TempoDescanso = request.TempoDescanso,
                        Observacao = request.Observacao
                    };
                    _context.TreinoExercicios.Add(treinoExercicio);
                }

                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();

            // Log da criação do treino
            await _logFunction.LogOperation("CREATE", "Treinos", treino.Id, null, "SUCCESS", $"Treino '{treino.Nome}' criado.");

            return CreatedAtAction(nameof(GetTreino), new { id = treino.Id }, treino);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            await _logFunction.LogOperation("CREATE", "Treinos", null, null, "ERROR", $"Erro ao criar treino: {ex.Message}");
            return StatusCode(500, $"Erro ao criar treino: {ex.Message}");
        }
    }

    // Atualizar treino
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> AtualizarTreino(int id, [FromBody] Treino treinoAtualizado)
    {
        if (id != treinoAtualizado.Id)
        {
            return BadRequest("ID do treino não corresponde.");
        }

        var treino = await _context.Treinos
            .Include(t => t.TreinosExercicios)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (treino == null)
        {
            return NotFound("Treino não encontrado.");
        }

        try
        {
            treino.Nome = treinoAtualizado.Nome;
            treino.Repeticoes = treinoAtualizado.Repeticoes;
            treino.DiasTreino = treinoAtualizado.DiasTreino;
            treino.PesoUsado = treinoAtualizado.PesoUsado;
            treino.Observacao = treinoAtualizado.Observacao;

            _context.TreinoExercicios.RemoveRange(treino.TreinosExercicios);
            if (treinoAtualizado.TreinosExercicios != null)
            {
                foreach (var novoTreinoExercicio in treinoAtualizado.TreinosExercicios)
                {
                    novoTreinoExercicio.TreinoId = treino.Id;
                    _context.TreinoExercicios.Add(novoTreinoExercicio);
                }
            }

            await _context.SaveChangesAsync();

            // Log da atualização do treino
            await _logFunction.LogOperation("UPDATE", "Treinos", treino.Id, null, "SUCCESS", $"Treino '{treino.Nome}' atualizado.");

            return NoContent();
        }
        catch (Exception ex)
        {
            await _logFunction.LogOperation("UPDATE", "Treinos", treino.Id, null, "ERROR", $"Erro ao atualizar treino: {ex.Message}");
            return StatusCode(500, $"Erro ao atualizar treino: {ex.Message}");
        }
    }

    // Deletar treino
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeletarTreino(int id)
    {
        var treino = await _context.Treinos
            .Include(t => t.TreinosExercicios)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (treino == null)
        {
            return NotFound("Treino não encontrado.");
        }

        try
        {
            _context.TreinoExercicios.RemoveRange(treino.TreinosExercicios);
            _context.Treinos.Remove(treino);

            await _context.SaveChangesAsync();

            // Log da exclusão do treino
            await _logFunction.LogOperation("DELETE", "Treinos", treino.Id, null, "SUCCESS", $"Treino '{treino.Nome}' deletado.");

            return Ok("Treino deletado com sucesso.");
        }
        catch (Exception ex)
        {
            await _logFunction.LogOperation("DELETE", "Treinos", treino.Id, null, "ERROR", $"Erro ao deletar treino: {ex.Message}");
            return StatusCode(500, $"Erro ao deletar treino: {ex.Message}");
        }
    }
    // Obter treinos por FichaId
    [HttpGet("ficha/{fichaId}")]
    [Authorize]
    public async Task<ActionResult> GetTreinosByFichaId(int fichaId)
    {
        var treinos = await _context.Treinos
            .Where(t => t.FichaId == fichaId)
            .Include(t => t.TreinosExercicios)
                .ThenInclude(te => te.Exercicio)
            .ToListAsync();

        if (treinos == null || !treinos.Any())
        {
            return NotFound("Nenhum treino encontrado para a ficha especificada.");
        }

        var resultado = treinos.Select(t => new
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
        });

        return Ok(resultado);
    }

}
