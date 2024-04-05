using api.core.Data.Requests;

namespace api.core.services.abstractions;

public interface IModeratorService
{
    ModeratorResponseDTO CreateModerator(string apiKey, ModeratorCreateRequestDTO req);
}
