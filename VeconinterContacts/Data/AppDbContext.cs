using Microsoft.EntityFrameworkCore;
using VeconinterContacts.Models;

namespace VeconinterContacts.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<SubCliente> SubClientes => Set<SubCliente>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.SubClientes)
                .WithOne(sc => sc.Cliente!)
                .HasForeignKey(sc => sc.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<SubCliente>()
                .HasIndex(sc => sc.Email)
                .IsUnique();
        }
    }
}
