using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SensorSim.Domain;

namespace SensorSim.Infrastructure.Repositories;

public interface IRepository<T> where T : Entity
{
    T? Get(Expression<Func<T, bool>> predicate);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    
    void SaveChanges();
}

public abstract class Repository<T> : IRepository<T> where T : Entity
{
    private readonly DataContext _context;
    protected readonly DbSet<T> _set;

    public Repository(DataContext context)
    {
        _context = context;
        _set = context.Set<T>();
    }

    public virtual T? Get(Expression<Func<T, bool>> predicate)
    {
        return _set.FirstOrDefault(predicate);
    }

    public virtual IEnumerable<T> GetAll()
    {
        return _set;
    }

    public virtual void Add(T entity)
    {
        _set.Add(entity);
        _context.SaveChanges();
    }

    public virtual void Update(T entity)
    {
        _context.SaveChanges();
    }

    public virtual void Delete(T entity)
    {
        _set.Remove(entity);
        _context.SaveChanges();
    }
    
    public virtual void SaveChanges()
    {
        _context.SaveChanges();
    }
}