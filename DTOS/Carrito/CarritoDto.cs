namespace TiendaOnline.DTOS
{
    public class CarritoDto
    {
        public int CartId { get; set; }

        public int UserId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastUpdated { get; set; }

        public decimal? TotalAmount { get; set; }
        public List<CarritoItemDto> CarritoItems { get; set; } = [];
    }
}