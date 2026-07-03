namespace ExaminationSystem.Domain.Repositories;

public interface IGeneralRepository<T> where T : class
{
    IEnumerable<T> GetAll();

    T? GetById(int id);
    T? GetById(Guid id);

    IQueryable<T> GetTable();

    void Add(T entity);

    void Update(T entity);

    void Delete(T entity);

    Task<int> SaveChangesAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}