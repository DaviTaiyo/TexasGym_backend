using System;
using System.ComponentModel.DataAnnotations;

namespace texasgym_backend.Models
{
    public class Treinos
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string LinkYoutube { get; set; }

        public ICollection<TreinoFicha> TreinoFichas { get; set; }
    }
}

    //public class Treinos
    //{
    //    [Key]
    //    public int Id { get; set; }

    //    [Required]
    //    public int UsuarioId { get; set; }

    //    [Required]
    //    public DateTime DataTreino { get; set; }

    //    [Range(0, int.MaxValue, ErrorMessage = "Horas treinadas deve ser um valor positivo")]
    //    public int HorasTreinadas { get; set; }

    //    [Range(0, double.MaxValue, ErrorMessage = "Calorias queimadas deve ser um valor positivo")]
    //    public decimal CaloriasQueimadas { get; set; }

    //    [Range(0, int.MaxValue, ErrorMessage = "Séries deve ser um valor positivo")]
    //    public int Series { get; set; }

    //    [Range(0, int.MaxValue, ErrorMessage = "Repetições deve ser um valor positivo")]
    //    public int Repeticoes { get; set; }

    //    public string ExemploTreino { get; set; }

    //    [Range(0, int.MaxValue, ErrorMessage = "Tempo de descanso deve ser um valor positivo")]
    //    public int TempoDescanso { get; set; }

    //    public bool Finalizados { get; set; } = false;

    //    public ICollection<TreinoFicha> TreinoFichas { get; set; }
    //}
