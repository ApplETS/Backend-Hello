using api.core.Data.requests;
using api.core.Data.Responses;

namespace api.core.services.abstractions;

public interface IUserService
{
    public UserResponseDTO AddOrganizer(Guid id, UserCreateDTO organizerDto);

    public UserResponseDTO GetUser(Guid id);

    public IEnumerable<UserResponseDTO> GetUsers();

    public bool UpdateUser(Guid id, UserUpdateDTO dto);

    public bool ToggleUserActiveState(Guid id);

    public string UpdateUserAvatar(Guid id, IFormFile avatarFile);
}
