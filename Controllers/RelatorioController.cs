//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Threading.Tasks;
//using texasgym_backend.Models;
//using System.Linq;

//namespace texasgym_backend.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class RelatoriosController : ControllerBase
//    {
//        private readonly AppDbContext _context;

//        public RelatoriosController(AppDbContext context)
//        {
//            _context = context;
//        }

//        // Relatório: Fichas com Exercícios
//        [HttpGet("relatorio-fichas-exercicios")]
//        public async Task<IActionResult> RelatorioFichasExercicios()
//        {
//            var relatorio = await _context.Fichas
//                .Include(f => f.Exercicios)
//                .Select(f => new
//                {
//                    FichaId = f.Id,
//                    DataCriacao = f.DataCriacao,
//                    Observacao = f.Observacao,
//                    Exercicios = f.Exercicios.Select(e => new
//                    {
//                        NomeExercicio = e.Nome,
//                        Repeticoes = e.Repeticoes,
//                        PesoUsado = e.PesoUsado,
//                        TempoDescanso = e.TempoDescanso,
//                        Observacao = e.Observacao
//                    })
//                })
//                .ToListAsync();

//            return Ok(relatorio);
//        }

//        // Relatório: Fichas com Treinos
//        [HttpGet("relatorio-fichas-treinos")]
//        public async Task<IActionResult> RelatorioFichasTreinos()
//        {
//            var relatorio = await _context.Fichas
//                .Include(f => f.TreinoFichas)
//                .ThenInclude(tf => tf.Treino)
//                .Select(f => new
//                {
//                    FichaId = f.Id,
//                    DataCriacao = f.DataCriacao,
//                    Observacao = f.Observacao,
//                    Treinos = f.TreinoFichas.Select(tf => new
//                    {
//                        TreinoId = tf.Treino.Id,
//                        NomeTreino = tf.Treino.Nome,
//                        DescricaoTreino = tf.Treino.Descricao,
//                        LinkYoutube = tf.Treino.LinkYoutube
//                    })
//                })
//                .ToListAsync();

//            return Ok(relatorio);
//        }

//        // Relatório: Usuários com Medidas
//        [HttpGet("relatorio-usuarios-medidas")]
//        public async Task<IActionResult> RelatorioUsuariosMedidas()
//        {
//            var relatorio = await _context.Usuarios
//                .Include(u => u.Medidas)
//                .Select(u => new
//                {
//                    Nome = u.Nome,
//                    CPF = u.CPF,
//                    DataNascimento = u.DataNascimento,
//                    Medidas = u.Medidas.Select(m => new
//                    {
//                        Altura = m.Altura,
//                        Peso = m.Peso,
//                        GorduraCorporal = m.GorduraCorporal,
//                        DataMedida = m.DataMedida
//                    })
//                })
//                .ToListAsync();

//            return Ok(relatorio);
//        }
//    }
//}
