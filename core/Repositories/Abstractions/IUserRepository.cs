using api.core.Data.Entities;

namespace api.core.Repositories.Abstractions;

public interface IUserRepository
{
    public User Add(User entity);
    public bool Delete(User entity);
    public User? Get(string id);
    public IQueryable<User> GetAll();
    public bool Update(string id, User entity);
    public User? GetOrganizer(string id);
    public User? GetModerator(string id);
}
