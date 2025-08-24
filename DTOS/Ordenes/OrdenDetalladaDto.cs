using TiendaOnline.Models;

namespace TiendaOnline.DTOS
{
    public class OrdenDetalladaDto
    {
        public int OrderId { get; set; }

        public int CartId { get; set; }

        public string OrderNumber { get; set; } = null!;

        public DateTime? OrderDate { get; set; }

        public decimal OrderTotal { get; set; }

        public string? Status { get; set; }

        public string ShippingAddress { get; set; } = null!;

        public string BillingAddress { get; set; } = null!;
        public required User Usuario { get; set; }
        public List<OrdenItemDto> OrdenItems { get; set; } = [];
    }
}