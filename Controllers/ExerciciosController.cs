using Microsoft.AspNetCore.Mvc;
using texasgym_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using texasgym_backend.Data;
using texasgym_backend.Function;
using System.Linq;

namespace texasgym_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciciosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly LogFunction _logFunction;

        public ExerciciosController(AppDbContext context, LogFunction logFunction)
        {
            _context = context;
            _logFunction = logFunction;
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
                return NotFound("Exercício não encontrado.");
            }

            return exercicio;
        }

        // POST: api/Exercicios
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Exercicio>> PostExercicio(Exercicio exercicio)
        {
            try
            {
                _context.Exercicios.Add(exercicio);
                await _context.SaveChangesAsync();

                // Log de criação
                await _logFunction.LogOperation(
                    "CREATE",
                    "Exercicios",
                    exercicio.Id,
                    null, // Não está vinculado a um usuário específico
                    "SUCCESS",
                    $"Exercício {exercicio.Nome} criado com sucesso."
                );

                return CreatedAtAction(nameof(GetExercicio), new { id = exercicio.Id }, exercicio);
            }
            catch (Exception ex)
            {
                await _logFunction.LogOperation(
                    "CREATE",
                    "Exercicios",
                    null,
                    null,
                    "ERROR",
                    $"Erro ao criar exercício: {ex.Message}"
                );

                return StatusCode(500, $"Erro ao criar exercício: {ex.Message}");
            }
        }

        // PUT: api/Exercicios/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutExercicio(int id, Exercicio exercicioAtualizado)
        {
            if (id != exercicioAtualizado.Id)
            {
                return BadRequest("IDs não coincidem.");
            }

            try
            {
                _context.Entry(exercicioAtualizado).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // Log de atualização
                await _logFunction.LogOperation(
                    "UPDATE",
                    "Exercicios",
                    exercicioAtualizado.Id,
                    null,
                    "SUCCESS",
                    $"Exercício {exercicioAtualizado.Nome} atualizado."
                );

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExercicioExists(id))
                {
                    return NotFound("Exercício não encontrado.");
                }

                await _logFunction.LogOperation(
                    "UPDATE",
                    "Exercicios",
                    exercicioAtualizado.Id,
                    null,
                    "ERROR",
                    $"Erro de concorrência ao atualizar o exercício com ID {id}."
                );

                throw;
            }
            catch (Exception ex)
            {
                await _logFunction.LogOperation(
                    "UPDATE",
                    "Exercicios",
                    exercicioAtualizado.Id,
                    null,
                    "ERROR",
                    $"Erro ao atualizar exercício: {ex.Message}"
                );

                return StatusCode(500, $"Erro ao atualizar exercício: {ex.Message}");
            }
        }

        // DELETE: api/Exercicios/ID
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteExercicio(int id)
        {
            var exercicio = await _context.Exercicios.FindAsync(id);
            if (exercicio == null)
            {
                return NotFound("Exercício não encontrado.");
            }

            try
            {
                _context.Exercicios.Remove(exercicio);
                await _context.SaveChangesAsync();

                // Log de exclusão
                await _logFunction.LogOperation(
                    "DELETE",
                    "Exercicios",
                    exercicio.Id,
                    null,
                    "SUCCESS",
                    $"Exercício {exercicio.Nome} deletado."
                );

                return Ok("Exercício deletado com sucesso.");
            }
            catch (Exception ex)
            {
                await _logFunction.LogOperation(
                    "DELETE",
                    "Exercicios",
                    id,
                    null,
                    "ERROR",
                    $"Erro ao deletar exercício: {ex.Message}"
                );

                return StatusCode(500, $"Erro ao deletar exercício: {ex.Message}");
            }
        }

        // Obter exercícios por TreinoId
        [HttpGet("treino/{treinoId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Exercicio>>> GetExerciciosPorTreino(int treinoId)
        {
            var exercicios = await _context.TreinoExercicios
                .Where(te => te.TreinoId == treinoId)
                .Include(te => te.Exercicio)
                .Select(te => new
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
                .ToListAsync();

            if (!exercicios.Any())
            {
                return NotFound("Nenhum exercício encontrado para este treino.");
            }

            return Ok(exercicios);
        }

        private bool ExercicioExists(int id)
        {
            return _context.Exercicios.Any(e => e.Id == id);
        }
    }
}
