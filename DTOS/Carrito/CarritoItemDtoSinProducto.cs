namespace TiendaOnline.DTOS
{
    public class CarritoItemDtoSinProducto
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal? Subtotal { get; set; }

        public DateTime? AddedDate { get; set; }
    }
}