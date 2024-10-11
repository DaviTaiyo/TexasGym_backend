using System.ComponentModel.DataAnnotations;

namespace texasgym_backend.Models
{
    public class Treino
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        public string Descricao { get; set; }

        [StringLength(255)]
        public string LinkYoutube { get; set; }
    }
}
