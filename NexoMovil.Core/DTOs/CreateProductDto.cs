namespace NexoMovil.Core.DTOs
{
    public class CreateProductDto
    {
        public string Model { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
}