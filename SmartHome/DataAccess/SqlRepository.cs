using DataAccess.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class SqlRepository<T> where T : class
{
    private SmartHomeContext _database;
    private DbSet<T> _entities;
    
    private const string ElementNotFoundExceptionMessage = "Element not found";
    
    public SqlRepository(SmartHomeContext database)
    {
        _database = database;
        _entities = database.Set<T>();
    }
    
    public void Add(T element)
    {
        _entities.Add(element);
        _database.SaveChanges();
    }
    
    public T? GetById(long id)
    {
        return _entities.Find(id);
    }
    
    public List<T> GetAll()
    {
        return _entities.ToList();
    }
    
    public List<T> GetByFilter(Func<T, bool> filter)
    {
        return _entities.Where(filter).ToList();
    }
    
    public void Delete(T element)
    {
        bool elementExists = _entities.Any(e => e == element);
        if (!elementExists)
        {
            throw new ElementNotFoundException(ElementNotFoundExceptionMessage);
        }
        _entities.Remove(element);
        _database.SaveChanges();
    }
}