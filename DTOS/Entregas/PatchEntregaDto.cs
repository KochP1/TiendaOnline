namespace TiendaOnline.DTOS
{
    public class PatchEntregaDto
    {
        public string? Carrier { get; set; }

        public DateTime? ShippedDate { get; set; }

        public DateTime? EstimatedDelivery { get; set; }

        public DateTime? ActualDelivery { get; set; }

        public string? ShippingStatus { get; set; }
    }
}