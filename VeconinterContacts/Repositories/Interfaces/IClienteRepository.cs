using VeconinterContacts.Models;

namespace VeconinterContacts.Repositories.Interfaces
{
    public interface IClienteRepository : IGenericRepository<Cliente>
    {
        Task<Cliente?> GetWithSubsAsync(int id);
    }
}
