namespace api.core.Data.Entities;

[Flags]
public enum State
{
    OnHold = 1,
    Deleted = 2,
    Denied = 4,
    Approved = 8,
    Published = 16,
    All = OnHold | Deleted | Denied | Approved | Published
}
