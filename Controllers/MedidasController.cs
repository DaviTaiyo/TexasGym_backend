using Microsoft.AspNetCore.Mvc;
using texasgym_backend.Models;
using Microsoft.EntityFrameworkCore;

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
    public async Task<ActionResult<Medida>> CriarMedida(Medida medida)
    {
        // Verificar se o usuário existe antes de criar a medida
        var usuario = await _context.Usuarios.FindAsync(medida.UsuarioId);
        if (usuario == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        // Definir a data da medida como a data atual, se não foi definida
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

        // Verificar se a medida existe
        var medida = await _context.Medidas.FindAsync(id);
        if (medida == null)
        {
            return NotFound("Medida não encontrada.");
        }

        medida.Altura = medidaAtualizada.Altura;
        medida.Peso = medidaAtualizada.Peso;
        medida.GorduraCorporal = medidaAtualizada.GorduraCorporal;
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
