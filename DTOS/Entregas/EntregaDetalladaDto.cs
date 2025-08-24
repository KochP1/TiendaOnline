namespace TiendaOnline.DTOS
{
    public class EntregaDetalladaDto
    {
        public int ShippingId { get; set; }

        public string? TrackingNumber { get; set; }

        public string? Carrier { get; set; }

        public DateTime? ShippedDate { get; set; }

        public DateTime? EstimatedDelivery { get; set; }

        public DateTime? ActualDelivery { get; set; }

        public string? ShippingStatus { get; set; }
        public required OrdenDetalladaDto Orden { get; set; }
    }
}