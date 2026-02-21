using Crm.Sample.Domain.Entities.Customers;
using Crm.Sample.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Crm.Sample.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            AddConfiguration(modelBuilder);
        }

        private static void AddConfiguration(ModelBuilder modelBuilder)
        {
            _ = new CustomerConfiguration(modelBuilder);
        }
    }
}
