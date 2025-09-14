using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VeconinterContacts.Data;
using VeconinterContacts.Repositories.Interfaces;

namespace VeconinterContacts.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _ctx;
        protected readonly DbSet<T> _db;

        public GenericRepository(AppDbContext ctx) { _ctx = ctx; _db = ctx.Set<T>(); }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> q = _db;
            foreach (var inc in includes) q = q.Include(inc);
            return await q.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> q = _db;
            foreach (var inc in includes) q = q.Include(inc);
            // requiere propiedad "Id" convención; si quisieras evitar reflexión, haz repos específicos
            return await q.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public async Task AddAsync(T entity) => await _db.AddAsync(entity);
        public Task UpdateAsync(T entity) { _db.Update(entity); return Task.CompletedTask; }
        public Task DeleteAsync(T entity) { _db.Remove(entity); return Task.CompletedTask; }
        public async Task SaveAsync() => await _ctx.SaveChangesAsync();
    }
}
