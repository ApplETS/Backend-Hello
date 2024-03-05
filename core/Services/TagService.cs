using api.core.Data.Responses;
using api.core.repositories.abstractions;
using api.core.services.abstractions;

namespace api.core.Services;

public class TagService(ITagRepository repository) : ITagService
{
    public IEnumerable<TagResponseDTO> GetTags()
    {
        return repository.GetAll().Select(TagResponseDTO.Map);
    }
}
