using Microsoft.AspNetCore.Mvc;
using texasgym_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class MedidasController : ControllerBase
{
    private readonly AppDbContext _context;

    public MedidasController(AppDbContext context)
    {
        _context = context;
    }

    // Obter todas as medidas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Medida>>> GetMedidas()
    {
        return await _context.Medidas.ToListAsync();
    }

    // Obter uma medida específica por ID
    [HttpGet("{id}")]
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
    public async Task<IActionResult> CriarMedida([FromBody] Medida medida)
    {
        // Tentar encontrar o usuário pelo ID
        var usuario = await _context.Usuarios
                                    .FirstOrDefaultAsync(u => u.Id == medida.UsuarioId);

        if (usuario == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        // Definir a data da medida como a data atual, caso não seja fornecida
        if (medida.DataMedida == default)
        {
            medida.DataMedida = DateTime.Now;
        }

        _context.Medidas.Add(medida);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMedida), new { id = medida.Id }, medida);
    }


    // Atualizar uma medida existente
    [HttpPut("{id}")]
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

        // Atualizar os campos da medida
        medida.Altura = medidaAtualizada.Altura;
        medida.Peso = medidaAtualizada.Peso;
        medida.GorduraCorporal = medidaAtualizada.GorduraCorporal;

        // Se não houver data definida, atribui a data atual
        if (!medidaAtualizada.DataMedida.HasValue)
        {
            medidaAtualizada.DataMedida = DateTime.Now;
        }
        medida.DataMedida = medidaAtualizada.DataMedida;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Deletar uma medida
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarMedida(int id)
    {
        var medida = await _context.Medidas.FindAsync(id);
        if (medida == null)
        {
            return NotFound();
        }

        _context.Medidas.Remove(medida);
        await _context.SaveChangesAsync();

        return Ok("Medida deletada com sucesso.");
    }
}
