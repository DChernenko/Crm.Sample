using System.Linq.Expressions;

namespace Crm.Sample.Domain.Repositories.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        ValueTask<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        TEntity Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    }
}
