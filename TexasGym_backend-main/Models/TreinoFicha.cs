namespace texasgym_backend.Models
{
    public class TreinoFicha
    {
        public int Id { get; set; }
        public int FichaId { get; set; }
        public Fichas Ficha { get; set; }

        public int TreinoId { get; set; }
        public Treinos Treino { get; set; }
    }
}
