using TiendaOnline.Models;

namespace TiendaOnline.DTOS
{
    public class OrdenItemProductoDto
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal? Subtotal { get; set; }
        public required Product Producto { get; set; }
    }
}