namespace TiendaOnline.DTOS
{
    public class CarritoItemDto
    {
        public int CartItemId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal? Subtotal { get; set; }

        public DateTime? AddedDate { get; set; }
        public required ProductoDto Producto { get; set; }
    }
}