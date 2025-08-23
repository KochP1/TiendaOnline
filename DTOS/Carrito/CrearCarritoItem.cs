namespace TiendaOnline.DTOS
{
    public class CrearCarritoItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}