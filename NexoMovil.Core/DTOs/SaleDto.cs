namespace NexoMovil.Core.DTOs
{
    public class SaleDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
    }
}