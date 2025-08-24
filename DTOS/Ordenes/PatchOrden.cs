namespace TiendaOnline.DTOS
{
    public class PatchOrden
    {
        public decimal OrderTotal { get; set; }

        public string? Status { get; set; }

        public string ShippingAddress { get; set; } = null!;

        public string BillingAddress { get; set; } = null!;
    }
}