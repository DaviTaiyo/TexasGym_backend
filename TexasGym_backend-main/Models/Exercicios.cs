//namespace texasgym_backend.Models
//{
//    public class Exercicio
//    {
//        public int Id { get; set; }
//        public int FichaId { get; set; }
//        public Fichas Ficha { get; set; }
//        public string? Nome { get; set; }
//        public int Repeticoes { get; set; }
//        public string? DiasTreino { get; set; }
//        public decimal PesoUsado { get; set; }
//        public int TempoDescanso { get; set; }
//        public string? Observacao { get; set; }
//    }
//}
namespace texasgym_backend.Models
{
    public class Exercicio
    {
        public int Id { get; set; }
        public int FichaId { get; set; }
        public Fichas Ficha { get; set; }
        public string Nome { get; set; }
        public int Repeticoes { get; set; }
        public string DiasTreino { get; set; }
        public decimal PesoUsado { get; set; }
        public int TempoDescanso { get; set; }
        public string Observacao { get; set; }
    }
}

