namespace TiendaOnline.DTOS
{
    public class CrearPagoDto
    {

        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public string? PaymentMethod { get; set; }
    }
}