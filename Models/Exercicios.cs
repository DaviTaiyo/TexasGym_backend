using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace texasgym_backend.Models
{
    public class Exercicio
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        [Column("nome")]
        public string Nome { get; set; }

        [Column("descricao")]
        public string? Descricao { get; set; }

        [StringLength(255)]
        [Column("link_youtube")]
        public string? LinkYoutube { get; set; }

        [JsonIgnore]
        public ICollection<TreinoExercicio>? TreinosExercicios { get; set; } // Relacionamento com TreinoExercicio
    }
}
