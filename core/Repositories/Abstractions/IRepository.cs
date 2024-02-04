namespace api.core.repositories.abstractions;

public interface IRepository<T>
{
    public T Add(T entity);
    public bool Delete(T entity);
    public T? Get(Guid id);
    public IEnumerable<T> GetAll();
    public bool Update(Guid id, T entity);
}
