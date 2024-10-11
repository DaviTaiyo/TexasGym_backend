using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public async Task<ActionResult<IEnumerable<Ficha>>> GetFichas()
    {
        return await _context.Fichas.ToListAsync();
    }

    // Obter ficha por ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Ficha>> GetFicha(int id)
    {
        var ficha = await _context.Fichas.FindAsync(id);

        if (ficha == null)
        {
            return NotFound();
        }

        return ficha;
    }

    // Criar nova ficha
    [HttpPost]
    public async Task<ActionResult<Ficha>> CriarFicha(Ficha ficha)
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
