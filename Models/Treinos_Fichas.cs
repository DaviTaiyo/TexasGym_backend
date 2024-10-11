using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace texasgym_backend.Models
{
    public class TreinoFicha
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Ficha")]
        public int FichaId { get; set; }
        public Ficha Ficha { get; set; }

        [Required]
        [ForeignKey("Treino")]
        public int TreinoId { get; set; }
        public Treino Treino { get; set; }
    }
}
