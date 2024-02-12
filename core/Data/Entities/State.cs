namespace api.core.Data.Entities;

[Flags]
public enum State
{
    OnHold = 0,
    Deleted = 1,
    Denied = 2,
    Approved = 4,
    Published = 8,
    All = OnHold | Deleted | Denied | Approved | Published
}
