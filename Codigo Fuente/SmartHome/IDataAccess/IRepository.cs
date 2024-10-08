namespace IDataAccess;

public interface IRepository<T>
{
    void Add(T element);
    
    T? GetById(long id);

    List<T> GetByFilter(Func<T, bool> filter, PageData? pageData);
    
    List<T> GetAll(PageData? pageData);
    
    void Delete(T element);
    
    void Update(T element);
}