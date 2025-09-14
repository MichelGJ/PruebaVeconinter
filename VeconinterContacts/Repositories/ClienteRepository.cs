using Microsoft.EntityFrameworkCore;
using VeconinterContacts.Data;
using VeconinterContacts.Models;
using VeconinterContacts.Repositories.Interfaces;

namespace VeconinterContacts.Repositories
{
    public class ClienteRepository : GenericRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(AppDbContext ctx) : base(ctx) { }

        public Task<Cliente?> GetWithSubsAsync(int id) =>
            _ctx.Clientes.Include(c => c.SubClientes)
                         .FirstOrDefaultAsync(c => c.Id == id);
    }
}
