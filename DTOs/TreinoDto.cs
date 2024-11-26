using System.Collections.Generic;

namespace texasgym_backend.DTOs
{
    public class CriarTreinoDto
    {
        public int FichaId { get; set; }
        public string Nome { get; set; }
        public int Repeticoes { get; set; }
        public string DiasTreino { get; set; }
        public decimal PesoUsado { get; set; }
        public int TempoDescanso { get; set; }
        public string Observacao { get; set; }
        public List<int> Exercicios { get; set; } // Lista de IDs de exercícios
    }
}
