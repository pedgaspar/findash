namespace Findash.Abstractions;

public interface IRepository<T>
{
    Task<T?> GetById(Guid id);
    Task<IEnumerable<T>> GetAll(int? page = 1, int? numberOfRecords = 100, string? contains = "");
    Task Update(T existingEntity);
    Task Create(T entity);
    Task Delete(T entity);
}