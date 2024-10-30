using System.ComponentModel.DataAnnotations;

namespace texasgym_backend.DTOs
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }
    }
}
