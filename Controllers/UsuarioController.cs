using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using texasgym_backend.Data;
using texasgym_backend.Models;
using texasgym_backend.DTOs;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtTokenGenerator _tokenGenerator;

    public UsuarioController(AppDbContext context, JwtTokenGenerator? tokenGenerator)
    {
        _context = context;
        _tokenGenerator = tokenGenerator;
    }

    private async Task LogOperation(string operationType, string tableName, int recordId, string details)
    {
        var log = new texasgym_backend.Models.Log
        {
            OperationType = operationType,
            TableName = tableName,
            RecordId = recordId,
            Status = "SUCCESS",
            Timestamp = DateTime.Now,
            Details = details
        };

        _context.Logs.Add(log);
        await _context.SaveChangesAsync();
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] Usuario usuario)
    {
        if (_context.Usuarios.Any(u => u.Email == usuario.Email || u.Telefone == usuario.Telefone || u.CPF == usuario.CPF))
            return BadRequest("Email, Telefone ou CPF já registrado!");

        usuario.Senha = UsuarioHelper.GerarHashDaSenha(usuario.Senha);

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        await LogOperation("CREATE", "usuarios", usuario.Id, $"Usuário {usuario.Nome} criado.");

        return Ok("Usuário registrado com sucesso.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Senha, usuario.Senha))
            return Unauthorized("Usuário ou senha inválidos.");

        var token = _tokenGenerator.GenerateToken(usuario);

        return Ok(new
        {
            Token = token,
            Professor = usuario.Professor
        });
    }

    [HttpPut("atualizar/{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] UsuarioUpdateDTO dadosAtualizados)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound("Usuário não encontrado.");

        usuario.Nome = dadosAtualizados.Nome;
        usuario.Email = dadosAtualizados.Email;
        usuario.Telefone = dadosAtualizados.Telefone;
        usuario.CPF = dadosAtualizados.CPF;
        usuario.DataNascimento = dadosAtualizados.DataNascimento;
        usuario.Professor = dadosAtualizados.Professor;

        await _context.SaveChangesAsync();

        await LogOperation("UPDATE", "usuarios", usuario.Id, $"Usuário {usuario.Nome} atualizado.");

        return Ok("Perfil atualizado.");
    }

    [HttpPut("atualizar-senha/{id}")]
    public async Task<IActionResult> AtualizarSenha(int id, [FromBody] string novaSenha)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound("Usuário não encontrado.");

        usuario.Senha = BCrypt.Net.BCrypt.HashPassword(novaSenha);

        await _context.SaveChangesAsync();

        await LogOperation("UPDATE", "usuarios", usuario.Id, $"Senha do usuário {usuario.Nome} atualizada.");

        return Ok("Senha atualizada.");
    }

    [HttpDelete("deletar/{id}")]
    [Authorize]
    public async Task<IActionResult> Deletar(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound("Usuário não encontrado.");

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        await LogOperation("DELETE", "usuarios", id, $"Usuário {usuario.Nome} deletado.");

        return Ok("Usuário deletado.");
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized("Token inválido.");

        var usuario = await _context.Usuarios
            .Where(u => u.Id.ToString() == userId)
            .Select(u => new
            {
                u.Id,
                u.Nome,
                u.Email,
                u.DataNascimento,
                u.Telefone,
                u.CPF,
                u.Administrador,
                u.Professor
            })
            .FirstOrDefaultAsync();

        if (usuario == null)
            return NotFound("Usuário não encontrado.");

        return Ok(usuario);
    }

    [HttpGet("listar")]
    [Authorize]
    public async Task<IActionResult> GetUsuarios()
    {
        var usuarios = await _context.Usuarios.ToListAsync();
        return Ok(usuarios);
    }
}
