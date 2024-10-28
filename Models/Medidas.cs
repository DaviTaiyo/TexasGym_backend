using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace texasgym_backend.Models
{
    public class Medida
    {
        public int Id { get; set; }

        [Required]
        [Column("Usuario_Id")]
        public int UsuarioId { get; set; }

        [Required]
        [Range(0, 3)]
        public decimal Altura { get; set; }

        [Required]
        [Range(0, 500)]
        public decimal Peso { get; set; }

        [Required]
        [Range(0, 100)]
        [Column("gordura_corporal")]
        public decimal GorduraCorporal { get; set; }

        [Column("data_medida")]
        public DateTime? DataMedida { get; set; }
    }
}
