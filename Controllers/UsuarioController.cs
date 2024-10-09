// Controllers/UsuarioController.cs
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using texasgym_backend.Data;
using texasgym_backend.Models;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuarioController(AppDbContext context)
    {
        _context = context;
    }

    // Criar (Registrar) um novo usuário
    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] Usuario usuario)
    {
        // Verificar se o email já existe
        if (_context.Usuarios.Any(u => u.Email == usuario.Email))
            return BadRequest("Email já registrado!");

        // Hashear a senha antes de armazenar
        usuario.Senha = UsuarioHelper.GerarHashDaSenha(usuario.Senha);

        // Adicionar o novo usuário ao banco
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return Ok("Usuário registrado com sucesso.");
    }

    // Login do usuário
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Usuario credenciais)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == credenciais.Email);

        if (usuario == null || !UsuarioHelper.VerificarSenha(credenciais.Senha, usuario.Senha))
            return Unauthorized("Credenciais inválidas!");

        return Ok("Login bem-sucedido!");
    }

    // Atualizar perfil do usuário
    [HttpPut("atualizar/{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] Usuario dadosAtualizados)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound("Usuário não encontrado.");

        usuario.Nome = dadosAtualizados.Nome;
        usuario.Email = dadosAtualizados.Email;
        await _context.SaveChangesAsync();
        return Ok("Perfil atualizado.");
    }

    // Atualizar a senha do usuário
    [HttpPut("atualizar-senha/{id}")]
    public async Task<IActionResult> AtualizarSenha(int id, [FromBody] string novaSenha)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound("Usuário não encontrado.");

        usuario.Senha = BCrypt.Net.BCrypt.HashPassword(novaSenha);
        await _context.SaveChangesAsync();
        return Ok("Senha atualizada.");
    }

    // Deletar usuário
    [HttpDelete("deletar/{id}")]
    public async Task<IActionResult> Deletar(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound("Usuário não encontrado.");

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return Ok("Usuário deletado.");
    }
}
