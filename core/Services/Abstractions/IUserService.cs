using System.Diagnostics.Tracing;

using api.core.Data.Enums;
using api.core.Data.requests;
using api.core.Data.Responses;

namespace api.core.services.abstractions;

public interface IUserService
{
    public UserResponseDTO AddOrganizer(string id, UserCreateDTO organizerDto);

    public UserResponseDTO GetUser(string id);

    public string GetUserAvatarUrl(string id);

    public IEnumerable<UserResponseDTO> GetUsers(string? search, OrganizerAccountActiveFilter activeFilter, out int count);

    public bool UpdateUser(string id, UserUpdateDTO dto);

    public bool ToggleUserActiveState(string id);

    public string UpdateUserAvatar(string id, IFormFile avatarFile);
}
