using SensorSim.Domain;

namespace SensorSim.Infrastructure.Repositories;

public abstract class CrudMemoryRepository<T> where T : Entity
{
    private Dictionary<string, T> Data { get; } = new();

    public IEnumerable<T> GetAll()
    {
        return Data.Values;
    }

    public void Add(T entity)
    {
        Data.Add(entity.Id, entity);
    }

    public T? Get(string id)
    {
        return Data.GetValueOrDefault(id);
    }

    public T GetOrDefault(string id)
    {
        var value = Get(id);
        
        if (value == null)
        {
            value = (T)Activator.CreateInstance(typeof(T), id)!;
            Data[id] = value;
        }
        
        return value;
    }

    public T GetOrCreate(string id, Func<T> create)
    {
        if (Data.TryGetValue(id, out T? value)) return value;
        value = create();
        Data[id] = value;

        return value;
    }

    public void Update(string id, T entity)
    {
        Data[id] = entity;
    }

    public void Delete(string id)
    {
        Data.Remove(id);
    }

    public void DeleteAll(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            Data.Remove(entity.Id);
        }
    }
}