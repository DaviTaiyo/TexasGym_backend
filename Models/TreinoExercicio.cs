using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using texasgym_backend.Models;

[Table("treinos_exercicios")]
public class TreinoExercicio
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Treino")]
    [Column("treino_id")]
    public int TreinoId { get; set; }

    [ForeignKey("Exercicio")]
    [Column("exercicio_id")]
    public int ExercicioId { get; set; }

    [Column("repeticoes")]
    public int? Repeticoes { get; set; }

    [Column("peso")]
    public decimal? Peso { get; set; }

    [Column("tempo_descanso")]
    public int? TempoDescanso { get; set; }

    [Column("observacao")]
    public string? Observacao { get; set; }

    public Treino? Treino { get; set; }
    public Exercicio? Exercicio { get; set; }
}
