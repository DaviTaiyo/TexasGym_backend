using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using texasgym_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using texasgym_backend.Data;

namespace texasgym_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatorioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RelatorioController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Relatório Simples de Usuários
        [HttpGet("RelatorioUsuariosSimples")]
        [Authorize]
        public async Task<IActionResult> GetRelatorioUsuariosSimples()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Medidas)
                .Select(u => new
                {
                    u.Id,
                    u.Nome,
                    u.Email,
                    u.Telefone,
                    u.CPF,
                    Medidas = u.Medidas.Select(m => new
                    {
                        m.Altura,
                        m.Peso,
                        m.GorduraCorporal,
                        m.DataMedida
                    })
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        // 2. Relatório de Treinos por Usuário
        [HttpGet("RelatorioTreinosPorUsuario")]
        [Authorize]
        public async Task<ActionResult> GetRelatorioTreinosPorUsuario()
        {
            var relatorio = await _context.Usuarios
                .Include(u => u.Fichas)
                .ThenInclude(f => f.Treinos)
                .Select(u => new
                {
                    u.Nome,
                    Fichas = u.Fichas.Select(f => new
                    {
                        f.Id,
                        f.DataCriacao,
                        f.Observacao,
                        Treinos = f.Treinos.Select(t => new
                        {
                            t.Nome,
                            t.Repeticoes,
                            t.DiasTreino,
                            t.PesoUsado,
                            t.Observacao
                        })
                    })
                })
                .ToListAsync();

            return Ok(relatorio);
        }

        // 3. Relatório Completo de Usuários e Treinos
        [HttpGet("RelatorioCompleto")]
        [Authorize]
        public async Task<ActionResult> GetRelatorioCompleto()
        {
            var relatorioCompleto = await _context.Usuarios
                .Include(u => u.Fichas)
                    .ThenInclude(f => f.Treinos)
                        .ThenInclude(t => t.Exercicio)
                .Include(u => u.Medidas)
                .Select(u => new
                {
                    Usuario = new
                    {
                        u.Nome,
                        u.Email,
                        u.Telefone,
                        u.DataNascimento,
                        u.CPF
                    },
                    Medidas = u.Medidas.Select(m => new
                    {
                        m.Altura,
                        m.Peso,
                        m.GorduraCorporal,
                        m.DataMedida
                    }),
                    Fichas = u.Fichas.Select(f => new
                    {
                        f.DataCriacao,
                        f.Observacao,
                        Treinos = f.Treinos.Select(t => new
                        {
                            t.Nome,
                            t.Repeticoes,
                            t.DiasTreino,
                            t.PesoUsado,
                            t.TempoDescanso,
                            t.Observacao,
                            Exercicio = new
                            {
                                t.Exercicio.Nome,
                                t.Exercicio.Descricao,
                                t.Exercicio.LinkYoutube
                            }
                        })
                    })
                })
                .ToListAsync();

            return Ok(relatorioCompleto);
        }
    }
}
