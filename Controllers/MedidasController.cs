using Microsoft.AspNetCore.Mvc;
using texasgym_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using texasgym_backend.Data;
using texasgym_backend.Function;

[Route("api/[controller]")]
[ApiController]
public class MedidasController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly LogFunction _logFunction;

    public MedidasController(AppDbContext context, LogFunction logFunction)
    {
        _context = context;
        _logFunction = logFunction;
    }

    // Obter todas as medidas
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Medida>>> GetMedidas()
    {
        return await _context.Medidas.ToListAsync();
    }

    [HttpGet("usuario/{userId}/latest")]
    public async Task<ActionResult<Medida>> GetLatestMedida(int userId)
    {
        var medida = await _context.Medidas
                                   .Where(m => m.UsuarioId == userId)
                                   .OrderByDescending(m => m.DataMedida)
                                   .FirstOrDefaultAsync();

        if (medida == null)
        {
            return NotFound("Nenhuma medida encontrada.");
        }

        return Ok(medida);
    }

    // Obter uma medida específica por ID
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Medida>> GetMedida(int id)
    {
        var medida = await _context.Medidas.FindAsync(id);

        if (medida == null)
        {
            return NotFound();
        }

        return medida;
    }

    // Criar uma nova medida
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CriarMedida([FromBody] Medida medida)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == medida.UsuarioId);

        if (usuario == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        if (medida.DataMedida == default)
        {
            medida.DataMedida = DateTime.Now;
        }

        try
        {
            _context.Medidas.Add(medida);
            await _context.SaveChangesAsync();

            // Log da criação da medida
            await _logFunction.LogOperation("CREATE", "Medidas", medida.Id, medida.UsuarioId, "SUCCESS", $"Medida criada para o usuário {usuario.Nome}.");

            return CreatedAtAction(nameof(GetMedida), new { id = medida.Id }, medida);
        }
        catch (Exception ex)
        {
            await _logFunction.LogOperation("CREATE", "Medidas", null, medida.UsuarioId, "ERROR", $"Erro ao criar medida: {ex.Message}");
            return StatusCode(500, $"Erro ao criar medida: {ex.Message}");
        }
    }

    // Atualizar uma medida existente
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> AtualizarMedida(int id, Medida medidaAtualizada)
    {
        if (id != medidaAtualizada.Id)
        {
            return BadRequest();
        }

        var medida = await _context.Medidas.FindAsync(id);
        if (medida == null)
        {
            return NotFound("Medida não encontrada.");
        }

        try
        {
            medida.Altura = medidaAtualizada.Altura;
            medida.Peso = medidaAtualizada.Peso;
            medida.GorduraCorporal = medidaAtualizada.GorduraCorporal;
            medida.DataMedida = medidaAtualizada.DataMedida ?? DateTime.Now;

            await _context.SaveChangesAsync();

            // Log da atualização da medida
            await _logFunction.LogOperation("UPDATE", "Medidas", medida.Id, medida.UsuarioId, "SUCCESS", $"Medida atualizada para o usuário com ID {medida.UsuarioId}.");

            return NoContent();
        }
        catch (Exception ex)
        {
            await _logFunction.LogOperation("UPDATE", "Medidas", medida.Id, medida.UsuarioId, "ERROR", $"Erro ao atualizar medida: {ex.Message}");
            return StatusCode(500, $"Erro ao atualizar medida: {ex.Message}");
        }
    }

    // Deletar uma medida
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeletarMedida(int id)
    {
        var medida = await _context.Medidas.FindAsync(id);
        if (medida == null)
        {
            return NotFound("Medida não encontrada.");
        }

        try
        {
            _context.Medidas.Remove(medida);
            await _context.SaveChangesAsync();

            // Log da exclusão da medida
            await _logFunction.LogOperation("DELETE", "Medidas", medida.Id, medida.UsuarioId, "SUCCESS", $"Medida deletada para o usuário com ID {medida.UsuarioId}.");

            return Ok("Medida deletada com sucesso.");
        }
        catch (Exception ex)
        {
            await _logFunction.LogOperation("DELETE", "Medidas", medida.Id, medida.UsuarioId, "ERROR", $"Erro ao deletar medida: {ex.Message}");
            return StatusCode(500, $"Erro ao deletar medida: {ex.Message}");
        }
    }
}
