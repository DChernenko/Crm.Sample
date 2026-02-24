using Crm.Sample.Application.Common.Interfaces;

namespace Crm.Sample.Infrastructure.Persistence
{
    public class UnitOfWork(AppDbContext context): IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => context.SaveChangesAsync(cancellationToken);
    }
}
