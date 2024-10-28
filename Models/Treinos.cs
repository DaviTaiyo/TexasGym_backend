using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace texasgym_backend.Models
{
    public class Treino
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Ficha")]
        [Column ("ficha_id")]
        public int FichaId { get; set; }

        [ForeignKey("Exercicio")]
        [Column ("exercicios_id")]
        public int ExerciciosId { get; set; }

        [Required, StringLength(100)]
        [Column ("nome")]
        public string Nome { get; set; }

        [Column ("repeticoes")]
        public int? Repeticoes { get; set; }

        [StringLength(50)]
        [Column ("dias_treino")]
        public string? DiasTreino { get; set; }

        [Column ("peso_usado")]
        public decimal? PesoUsado { get; set; }

        [Column ("tempo_descanso")]
        public int? TempoDescanso { get; set; }

        [Column ("observacao")]
        public string? Observacao { get; set; }

        [JsonIgnore]
        public Exercicio? Exercicio { get; set; }

        [JsonIgnore]
        public Ficha? Ficha { get; set; }
    }
}
