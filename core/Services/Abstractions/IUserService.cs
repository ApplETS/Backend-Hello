using api.core.Data.requests;
using api.core.Data.Responses;

namespace api.core.services.abstractions;

public interface IUserService
{
    public UserResponseDTO AddOrganizer(UserRequestDTO organizerDto);

    public UserResponseDTO GetUser(Guid id);

    public bool UpdateUser(Guid id, UserRequestDTO dto);
}
