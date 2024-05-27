namespace api.core.Data.Requests;

public class ModeratorCreateRequestDTO
{
    /// <summary>
    /// This id when passed needs to be already created in supabase.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// A email that will be bound to the moderator, make sure that this email match the 
    /// user in supabase of the given id. 
    /// </summary>
    public required string Email { get; set; }
}
