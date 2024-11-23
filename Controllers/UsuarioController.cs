using Microsoft.AspNetCore.Mvc;
using texasgym_backend.Models;
using Microsoft.EntityFrameworkCore;
using texasgym_backend.DTOs;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using texasgym_backend.Data;
using System.Security.Claims;

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

    // Criar (Registrar) um novo usuário
    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] Usuario usuario)
    {
        // Verificar se o email ou telefone ou CPF já existem
        if (_context.Usuarios.Any(u => u.Email == usuario.Email || u.Telefone == usuario.Telefone || u.CPF == usuario.CPF))
            return BadRequest("Email, Telefone ou CPF já registrado!");

        // Hashear a senha antes de armazenar
        usuario.Senha = UsuarioHelper.GerarHashDaSenha(usuario.Senha);

        // Adicionar o novo usuário ao banco
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
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
        return Ok(new { Token = token });
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
        usuario.Telefone = dadosAtualizados.Telefone;
        usuario.CPF = dadosAtualizados.CPF;
        usuario.DataNascimento = dadosAtualizados.DataNascimento;

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
    [Authorize]
    public async Task<IActionResult> Deletar(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound("Usuário não encontrado.");

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return Ok("Usuário deletado.");
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return Unauthorized("Token inválido.");
        }

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
                u.Administrador
            })
            .FirstOrDefaultAsync();

        if (usuario == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        return Ok(usuario);
    }


    // Listar todos os usuários
    [HttpGet("listar")]
    [Authorize]
    public async Task<IActionResult> GetUsuarios()
    {
        // Busca todos os usuários do banco de dados
        var usuarios = await _context.Usuarios.ToListAsync();

        // Retorna a lista de usuários
        return Ok(usuarios);
    }
}
