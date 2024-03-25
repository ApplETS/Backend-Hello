using System.Text.Json.Serialization;

namespace api.core.Data.Enums;

/// <summary>
/// 1 - OnHold
/// 2 - Deleted
/// 4 - Denied
/// 8 - Approved
/// 16 - Published
/// 32 - Draft
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
[Flags]
public enum State
{
    OnHold = 1,
    Deleted = 2,
    Denied = 4,
    Approved = 8,
    Published = 16,
    Draft = 32,
    All = OnHold | Deleted | Denied | Approved | Published | Draft
}
