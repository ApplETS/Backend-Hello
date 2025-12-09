using api.core.Data.Entities;

namespace api.core.Repositories.Abstractions;

public interface IUserRepository<T> where T : User
{
    public T Add(T entity);
    public bool Delete(T entity);
    public T? Get(string id);
    public IQueryable<T> GetAll();
    public bool Update(string id, T entity);
}
