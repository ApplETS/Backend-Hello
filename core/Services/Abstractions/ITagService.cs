using api.core.data.entities;
using api.core.Data.Responses;

namespace api.core.services.abstractions;

public interface ITagService
{
    public IEnumerable<TagResponseDTO> GetTags();
    public ICollection<Tag> GetAssociatedTags(ICollection<Guid> tagIds);
}
