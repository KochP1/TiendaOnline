using System.ComponentModel.DataAnnotations;

namespace TiendaOnline.DTOS
{
    public class CredencialesUsuarioDto
    {
        [Required]
        public required string Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}