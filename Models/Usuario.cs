using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace texasgym_backend.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Nome { get; set; }

        [DataType(DataType.Date)]
        [Column("data_nascimento")]
        public DateTime DataNascimento { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(11)]
        public string CPF { get; set; }

        [StringLength(15)]
        public string Telefone { get; set; }

        [Required]
        public string? Senha { get; set; }

        [Column("professor")]
        public bool Professor { get; set; } = false;

        [Column("administrador")]
        public bool Administrador { get; set; } = false;
        [JsonIgnore]
        public ICollection<Ficha>? Fichas { get; set; }
        [JsonIgnore]
        public ICollection<Medida>? Medidas { get; set; }
    }
}
