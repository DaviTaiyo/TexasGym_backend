//namespace texasgym_backend.Models
//{
//    public class Fichas
//    {
//        public int Id { get; set; }
//        public int UsuarioId { get; set; }
//        public string? Nome { get; set; }
//        public string? Repeticao { get; set; }
//        public string? Descanso { get; set; }
//        public int Peso { get; set; }
//        public string? Observacao { get; set; }
//        public Usuario Usuario { get; set; }
//        public ICollection<Exercicio> Exercicios { get; set; }
//        public ICollection<TreinoFicha> TreinoFichas { get; set; }
//    }
//}

namespace texasgym_backend.Models
{
    public class Fichas
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public string Observacao { get; set; }

        public ICollection<Exercicio> Exercicios { get; set; }
        public ICollection<TreinoFicha> TreinoFichas { get; set; }
    }
}
