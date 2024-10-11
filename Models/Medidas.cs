using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace texasgym_backend.Models
{
    public class Medida
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        [Range(0, 300)]
        public decimal? Altura { get; set; }

        [Range(0, 500)]
        public decimal? Peso { get; set; }

        [Range(0, 100)]
        public decimal? GorduraCorporal { get; set; }

        [Required]
        public DateTime DataMedida { get; set; }
    }
}
