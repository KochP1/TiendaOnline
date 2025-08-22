namespace TiendaOnline.DTOS
{
    public class RespuestaAutentificacionDto
    {
        public required string Token { get; set; }
        public DateTime Expiracion { get; set; }
    }
}