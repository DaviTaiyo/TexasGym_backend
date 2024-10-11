using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace texasgym_backend.Models
{
    public class Exercicio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Ficha")]
        public int FichaId { get; set; }
        public Ficha Ficha { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        public int Repeticoes { get; set; }

        public string DiasTreino { get; set; }

        public decimal? PesoUsado { get; set; }

        public int? TempoDescanso { get; set; }

        public string Observacao { get; set; }
    }
}
