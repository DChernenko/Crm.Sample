namespace Crm.Sample.Application.Common.Interfaces
{
    // todo use IDisposable??
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
