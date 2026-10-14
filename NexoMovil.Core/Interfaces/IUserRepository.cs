using NexoMovil.Core.Entities;

namespace NexoMovil.Core.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByCredentialsAsync(string email, string hashedPassword);
    }
}