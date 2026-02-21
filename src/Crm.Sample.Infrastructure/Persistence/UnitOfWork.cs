using Crm.Sample.Application.Common.Interfaces;

namespace Crm.Sample.Infrastructure.Persistence
{
    //todo: where is located?
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context) => _context = context;

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => _context.SaveChangesAsync(cancellationToken);

        public void Dispose() => _context.Dispose();
    }
}
