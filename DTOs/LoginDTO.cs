using System.ComponentModel.DataAnnotations;

namespace texasgym_backend.DTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }  // Email obrigatório

        [Required]
        public string Senha { get; set; }  // Senha obrigatória
    }
}
