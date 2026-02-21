namespace Crm.Sample.Application.Abstractions.Base
{
    public interface IBaseService<TEntity, TCreateDto, TUpdateDto, TResponseDto>
         where TEntity : class
         where TResponseDto : class
    {
        Task<TResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TResponseDto> CreateAsync(TCreateDto createDto, int userId, CancellationToken cancellationToken = default);
        Task<TResponseDto> UpdateAsync(int id, TUpdateDto updateDto, int userId, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
