using api.core.data.entities;

namespace api.core.Data.Responses;

public class TagResponseDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public static TagResponseDTO Map(Tag oneTag)
    {
        return new TagResponseDTO
        {
            Id = oneTag.Id,
            Name = oneTag.Name,
            CreatedAt = oneTag.CreatedAt,
            UpdatedAt = oneTag.UpdatedAt
        };
    }
}

public class TagDetailsResponseDTO : TagResponseDTO
{
    public int PriorityValue { get; set; }

    public IEnumerable<TagDetailsResponseDTO> Children { get; set; }

    public static TagDetailsResponseDTO Map(Tag oneTag)
    {
        return new TagDetailsResponseDTO
        {
            Id = oneTag.Id,
            Name = oneTag.Name,
            Children = oneTag.ChildrenTags.Select(Map),
            PriorityValue = oneTag.PriorityValue,
            CreatedAt = oneTag.CreatedAt,
            UpdatedAt = oneTag.UpdatedAt
        };
    }
}

