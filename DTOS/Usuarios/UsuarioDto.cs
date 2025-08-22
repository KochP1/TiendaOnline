namespace TiendaOnline.DTOS
{
    public class UsuarioDto
    {
        public int UserId { get; set; }

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public DateTime? LastLogin { get; set; }

        public bool? IsActive { get; set; }
    }
}