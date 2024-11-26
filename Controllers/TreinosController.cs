using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using texasgym_backend.Data;
using texasgym_backend.DTOs;
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

        Console.WriteLine($"Treino encontrado: {treino.Id}");
        foreach (var te in treino.TreinosExercicios)
        {
            Console.WriteLine($"Exercício: {te.Exercicio?.Nome}, ID: {te.ExercicioId}");
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

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CriarTreino([FromBody] CriarTreinoDto request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Criar o treino principal
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

            // Criar as associações com os exercícios
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

            // Confirmar a transação
            await transaction.CommitAsync();

            // Retornar o resultado
            return CreatedAtAction(nameof(GetTreino), new { id = treino.Id }, new
            {
                treino.Id,
                treino.Nome,
                treino.Repeticoes,
                treino.DiasTreino,
                treino.PesoUsado,
                treino.TempoDescanso,
                treino.Observacao,
                Exercicios = request.Exercicios.Select(id => new { Id = id })
            });
        }
        catch (Exception ex)
        {
            // Reverter a transação em caso de erro
            await transaction.RollbackAsync();
            return StatusCode(500, $"Erro ao criar treino: {ex.Message}");
        }
    }


    // Atualizar treino existente
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> AtualizarTreino(int id, [FromBody] Treino treinoAtualizado)
    {
        if (id != treinoAtualizado.Id)
        {
            return BadRequest();
        }

        var treino = await _context.Treinos
            .Include(t => t.TreinosExercicios)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (treino == null)
        {
            return NotFound("Treino não encontrado.");
        }

        // Atualiza os dados do treino
        treino.Nome = treinoAtualizado.Nome;
        treino.Repeticoes = treinoAtualizado.Repeticoes;
        treino.DiasTreino = treinoAtualizado.DiasTreino;
        treino.PesoUsado = treinoAtualizado.PesoUsado;
        treino.Observacao = treinoAtualizado.Observacao;

        // Remove exercícios antigos e adiciona os novos
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
        return NoContent();
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

        // Remove os exercícios associados ao treino
        _context.TreinoExercicios.RemoveRange(treino.TreinosExercicios);

        // Remove o treino
        _context.Treinos.Remove(treino);
        await _context.SaveChangesAsync();

        return Ok("Treino deletado com sucesso.");
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
                te.Exercicio.LinkYoutube
            })
        });

        return Ok(resultado);
    }

}
