namespace TiendaOnline.DTOS
{
    public class CrearProductoDto
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string? Category { get; set; }

        public bool? IsActive { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}