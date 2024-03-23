namespace api.core.Misc;

public enum AuthPolicy
{
    IsModerator,
    OrganizerIsActive
}

public static class AuthPolicies
{
    public const string IsModerator = nameof(AuthPolicy.IsModerator);
    public const string OrganizerIsActive = nameof(AuthPolicy.OrganizerIsActive);
}