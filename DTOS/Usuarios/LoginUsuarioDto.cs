using System.ComponentModel.DataAnnotations;

namespace TiendaOnline.DTOS
{
    public class LoginUsuarioDto
    {
        [Required]
        public required string Email { get; set; }
        public required string? Password { get; set;}
    }
}