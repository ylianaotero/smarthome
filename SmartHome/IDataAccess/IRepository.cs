namespace IDataAccess;

public interface IRepository<T>
{
    void Add(T element);
    
    T? GetById(long id);

    List<T> GetByFilter(Func<T, bool> filter);
    
    List<T> GetAll();
    
    void Delete(T element);
    
    void Update(T element);
}