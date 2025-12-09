using api.core.data.entities;
using api.core.Repositories.Abstractions;

namespace api.core.repositories.abstractions;

public interface IModeratorRepository : IUserRepository<Moderator>
{
}
