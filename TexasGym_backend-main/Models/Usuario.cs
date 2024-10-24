using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace texasgym_backend.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public int Telefone { get; set; }
        public string CPF { get; set; }
        public string Senha { get; set; }
        public bool EhProfessor { get; set; } = false;
        public bool EhAdministrador { get; set; } = false;

        public ICollection<Fichas> Fichas { get; set; }
        public ICollection<Medida> Medidas { get; set; }
    }
}


//namespace texasgym_backend.Models
//{
//    public class Usuario
//    {
//        [Key]
//        public int Id { get; set; }
//        public string Nome { get; set; }
//        public DateTime DataNascimento { get; set; }
//        public string Email { get; set; }
//        public string Telefone { get; set; }
//        public string CPF { get; set; }
//        public string Senha { get; set; }
//        public bool EhProfessor { get; set; }
//        public bool EhAdministrador { get; set; }
//        public ICollection<Medida> Medidas { get; set; }
//        public ICollection<Fichas> Fichas { get; set; }
//    }
//}
