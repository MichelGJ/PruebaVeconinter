using VeconinterContacts.Data;
using VeconinterContacts.Models;
using VeconinterContacts.Repositories.Interfaces;

namespace VeconinterContacts.Repositories
{
    public class SubClienteRepository : GenericRepository<SubCliente>, ISubClienteRepository
    {
        public SubClienteRepository(AppDbContext ctx) : base(ctx) { }
    }
}
