using api.core.data.entities;
using api.core.Data.Responses;
using api.core.repositories.abstractions;
using api.core.services.abstractions;

namespace api.core.Services;

public class TagService(ITagRepository repository) : ITagService
{
    public IEnumerable<TagResponseDTO> GetTags()
    {
        return repository.GetAll()
            .OrderBy(x => x.Name)
            .Select(TagResponseDTO.Map);
    }

    public ICollection<Tag> GetAssociatedTags(ICollection<Guid> tagIds)
    {
        var tags = new List<Tag>();

        if (tagIds != null && tagIds.Count != 0)
        {
            tags = tagIds
                .Select(repository.Get)
                .ToList()!;
        }

        return tags;
    }
}
