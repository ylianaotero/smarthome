using System.Collections;
using System.Reflection;
using CustomExceptions;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class SqlRepository<T> : IRepository<T> where T : class
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
        List<T> elements = _entities.Where(filter).ToList();
        
        LoadEntities(elements);
        
        return elements;
    }
    
    public void Delete(T element)
    {
        bool elementExists = _entities.Any(e => e == element);
        if (!elementExists)
        {
            throw new ElementNotFound(ElementNotFoundExceptionMessage);
        }
        _entities.Remove(element);
        _database.SaveChanges();
    }

    public void Update(T element)
    {
        _entities.Attach(element);
        
        _database.Entry(element).State = EntityState.Modified;
        
        _database.SaveChanges();
    }
    
    private void LoadEntities(List<T> entities)
    {
        foreach (var element in entities)
        {
            List<PropertyInfo> properties = element.GetType().GetProperties().ToList();
            foreach (var property in properties)
            {
                bool isList = typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string);
                if (isList)
                {
                    LoadCollections(element, property);
                }
                else
                {
                    LoadReferences(element, property);
                }
            }
        }
    }
    
    private void LoadCollections(T element, PropertyInfo property)
    {
        var isNavigationCollection = _database.Entry(element).Metadata.FindNavigation(property.Name) != null;
        if (isNavigationCollection)
        {
            _database.Entry(element).Collection(property.Name).Load();
        }
    }
    
    private void LoadReferences(T element, PropertyInfo property)
    {
        var isNavigationProperty = _database.Entry(element).Metadata.FindNavigation(property.Name) != null;
        if (isNavigationProperty)
        {
            _database.Entry(element).Reference(property.Name).Load();
        }
    }
}