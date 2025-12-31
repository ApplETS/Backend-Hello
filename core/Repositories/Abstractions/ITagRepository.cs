using api.core.data.entities;
using api.core.Data.Responses;

namespace api.core.repositories.abstractions;

public interface ITagRepository : IRepository<Tag>
{
    public IEnumerable<FieldOfInterestTagResponseDTO> GetInterestFieldsForOrganizer(string organizerId, int take = 3);
}
