using Crm.Sample.Domain.Entities.Base;
using Crm.Sample.Domain.Repositories.Base;
using Crm.Sample.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Crm.Sample.Infrastructure.Repositories.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual ValueTask<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => _dbSet.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public virtual TEntity Add(TEntity entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
           => _dbSet.AnyAsync(predicate, cancellationToken);
    }
    //public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    //{
    //    protected readonly AppDbContext _context;
    //    protected readonly DbSet<T> _dbSet;

    //    protected BaseRepository(AppDbContext context)
    //    {
    //        _context = context;
    //        _dbSet = context.Set<T>();
    //    }

    //    public virtual ValueTask<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    //        => _dbSet.FindAsync(id, cancellationToken);

    //    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    //        => await _dbSet.ToListAsync(cancellationToken);

    //    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    //    {
    //        _dbSet.Add(entity);
    //        await _context.SaveChangesAsync(cancellationToken);
    //        return entity;
    //    }

    //    public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    //    {
    //        _dbSet.Update(entity);
    //        return _context.SaveChangesAsync(cancellationToken);
    //    }

    //    public virtual async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    //    {
    //        var entity = await GetByIdAsync(id, cancellationToken);
    //        if (entity != null)
    //        {
    //            _dbSet.Remove(entity);
    //            await _context.SaveChangesAsync(cancellationToken);
    //        }
    //    }

    //    public virtual Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    //       => _dbSet.AnyAsync(e => e.Id == id, cancellationToken);
    //}

    //public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    //{
    //    protected readonly IMongoCollection<T> _collection;

    //    protected BaseRepository(MongoDBContext context, string collectionName)
    //    {
    //        _collection = context.GetCollection<T>(collectionName);
    //    }

    //    public async Task<T?> GetByIdAsync(string id)
    //    {
    //        var filter = Builders<T>.Filter.Eq(x => x.Id, id);
    //        return await _collection.Find(filter).FirstOrDefaultAsync();
    //    }

    //    public async Task<IEnumerable<T>> GetAllAsync()
    //    {
    //        return await _collection.Find(_ => true).ToListAsync();
    //    }

    //    public async Task<T> AddAsync(T entity)
    //    {
    //        await _collection.InsertOneAsync(entity);
    //        return entity;
    //    }

    //    public async Task UpdateAsync(T entity)
    //    {
    //        var filter = Builders<T>.Filter.Eq(x => x.Id, entity.Id);
    //        await _collection.ReplaceOneAsync(filter, entity);
    //    }

    //    public async Task DeleteAsync(string id)
    //    {
    //        var filter = Builders<T>.Filter.Eq(x => x.Id, id);
    //        await _collection.DeleteOneAsync(filter);
    //    }

    //    public async Task<bool> ExistsAsync(string id)
    //    {
    //        var filter = Builders<T>.Filter.Eq(x => x.Id, id);
    //        return await _collection.Find(filter).AnyAsync();
    //    }
    //}
}
