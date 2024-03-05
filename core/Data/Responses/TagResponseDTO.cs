using api.core.data.entities;

namespace api.core.Data.Responses;

public class TagResponseDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public static TagResponseDTO Map(Tag tag) => new TagResponseDTO
    {
        Id = tag.Id,
        Name = tag.Name,
    };

}
