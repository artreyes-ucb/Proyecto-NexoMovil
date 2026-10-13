using NexoMovil.Core.Entities;

namespace NexoMovil.Core.Interfaces
{
    public interface ISaleRepository : IBaseRepository<Sale>
    {
        Task<Sale?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Sale>> GetAllWithDetailsAsync();
    }
}
