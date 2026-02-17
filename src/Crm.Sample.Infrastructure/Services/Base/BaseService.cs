using Crm.Sample.Application.Common.Enums;
using Crm.Sample.Application.Common.Interfaces;
using Crm.Sample.Application.Services.Base;
using Crm.Sample.Application.Services.Redis;
using Crm.Sample.Domain.Exceptions;
using Crm.Sample.Domain.Repositories.Base;
using Crm.Sample.Infrastructure.Options;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Crm.Sample.Infrastructure.Services.Base
{
    public abstract class BaseService<TEntity, TCreateDto, TUpdateDto, TResponseDto>
        : IBaseService<TEntity, TCreateDto, TUpdateDto, TResponseDto>
        where TEntity : class
        where TResponseDto : class
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IRedisCache _cache;
        protected readonly IValidator<TCreateDto>? _createValidator;
        protected readonly IValidator<TUpdateDto>? _updateValidator;
        protected readonly string _entityName;
        private readonly RedisOptions _redisOptions;

        protected BaseService(
            IBaseRepository<TEntity> repository,
            IUnitOfWork unitOfWork,
            IRedisCache cache,
            IOptions<RedisOptions> options,
            IValidator<TCreateDto>? createValidator = null,
            IValidator<TUpdateDto>? updateValidator = null)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _cache = cache;
            _redisOptions = options.Value;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _entityName = typeof(TEntity).Name;
        }

        public virtual async Task<TResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var cacheKey = GetCacheKey(CacheOperation.GetById, id);

            var cached = await _cache.GetAsync<TResponseDto>(cacheKey);
            if (cached != null)
                return cached;

            var entity = await _repository.GetByIdAsync(id, cancellationToken);

            if (entity == null)
                return null;

            var result = MapToResponse(entity);
            await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(_redisOptions.DefaulExpirationInMinutes));

            return result;
        }

        public virtual async Task<IEnumerable<TResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var cacheKey = GetCacheKey(CacheOperation.GetAll);

            var cached = await _cache.GetAsync<IEnumerable<TResponseDto>>(cacheKey);
            if (cached != null)
                return cached;

            var entities = await _repository.GetAllAsync(cancellationToken);

            var result = entities.Select(MapToResponse);

            await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(_redisOptions.DefaulExpirationInMinutes));

            return result;
        }
        
        public virtual async Task<TResponseDto> CreateAsync(TCreateDto createDto, int userId, CancellationToken cancellationToken = default)
        {
            // validation
            if (_createValidator != null)
                await _createValidator.ValidateAndThrowAsync(createDto, cancellationToken);
            
            // mapping DTO -> Entity
            var entity = MapToEntity(createDto, userId);
                        
            // add + save
            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // clear cache
            await _cache.RemoveAsync([GetCacheKey(CacheOperation.GetAll)]);

            // mapping Entity -> Response
            return MapToResponse(entity);
        }

        public virtual async Task<TResponseDto> UpdateAsync(int id, TUpdateDto updateDto, int userId, CancellationToken cancellationToken = default)
        {
            if (_updateValidator != null)
                await _updateValidator.ValidateAndThrowAsync(updateDto, cancellationToken);

            var entity = await _repository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException(typeof(TEntity).Name, id);

            MapToEntity(updateDto, entity, userId);

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            await _cache.RemoveAsync([GetCacheKey(CacheOperation.GetAll), GetCacheKey(CacheOperation.GetById, id)]);
            
            return MapToResponse(entity);
        }

        public virtual async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return;

            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _cache.RemoveAsync([GetCacheKey(CacheOperation.GetAll), GetCacheKey(CacheOperation.GetById, id)]);
        }

        protected abstract TResponseDto MapToResponse(TEntity entity);

        protected abstract TEntity MapToEntity(TCreateDto createDto, int userId);

        protected abstract void MapToEntity(TUpdateDto updateDto, TEntity entity, int userId);

        // todo move to utils??
        private string GetCacheKey(CacheOperation operation, params object[] parameters)
        {
            var key = $"{_entityName}:{operation}";
            if (parameters != null && parameters.Length > 0)
            {
                key += ":" + string.Join(":", parameters);
            }
            return key;
        }
    }
}
