namespace api.core.Data.Enums;

/// <summary>
/// Filter the organizer accounts by their active status
/// </summary>
[Flags]
public enum OrganizerAccountActiveFilter
{
    None = 0,
    Active = 1,
    Inactive = 2,
    All = Active | Inactive
}
