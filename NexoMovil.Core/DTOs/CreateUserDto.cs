namespace NexoMovil.Core.DTOs
{
    public class CreateUserDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Password { get; set; }
        public string? Telephone { get; set; }
        public string Role { get; set; } = null!;
    }
}