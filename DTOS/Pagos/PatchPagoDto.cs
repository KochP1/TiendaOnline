namespace TiendaOnline.DTOS
{
    public class PatchPagoDto
    {

        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public string? PaymentMethod { get; set; }
        
        public string? PaymentStatus { get; set; }
    }
}