using System.ComponentModel.DataAnnotations;
using texasgym_backend.DTOs;

namespace texasgym_backend.DTOs
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Senha { get; set; }
    }
}
