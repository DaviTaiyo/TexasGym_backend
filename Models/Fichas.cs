using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace texasgym_backend.Models
{
    public class Ficha
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        [Column ("Usuario_id")]
        public int UsuarioId { get; set; }

        [JsonIgnore]
        public Usuario? Usuario { get; set; }

        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Column("observacao")]
        public string Observacao { get; set; }

    }
}
