using Microsoft.EntityFrameworkCore;
using NexoMovil.Core.Entities;
using NexoMovil.Core.Interfaces;
using NexoMovil.Infrastructure.Data;

namespace NexoMovil.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(NexoMovilContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _entities.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByCredentialsAsync(string email, string hashedPassword)
        {
            return await _entities.FirstOrDefaultAsync(
                u => u.Email == email && u.Password == hashedPassword);
        }
    }
}