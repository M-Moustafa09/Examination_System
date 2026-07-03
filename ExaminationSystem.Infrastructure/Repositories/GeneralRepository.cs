using ExaminationSystem.Domain.Repositories;
using ExaminationSystem.Infrastructure.Persistence;

namespace ExaminationSystem.Infrastructure.Repositories;

public class GeneralRepository<T> : IGeneralRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;

    public GeneralRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public T? GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    public IQueryable<T> GetTable()
    {
        return _context.Set<T>();
    }

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        if (entity != null) _context.Set<T>().Remove(entity);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public T? GetById(Guid id)
    {
        return _context.Set<T>().Find(id);
    }
}