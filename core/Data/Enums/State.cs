using System.Text.Json.Serialization;

namespace api.core.Data.Enums;

/// <summary>
/// A flag enum to define state and intermediary state for filtering purpose.
/// 
/// Example of use:
/// filter only OnHold and Approved state
///     001001 -> 9
/// so you can pass 9 to the filter to get only OnHold and Approved
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
[Flags]
public enum State
{
    OnHold = 1 << 0, // 000001
    Deleted = 1 << 1, // 000010
    Denied = 1 << 2, // 000100
    Approved = 1 << 3, // 001000
    Published = 1 << 4, // 010000
    Draft = 1 << 5, // 100000
    All = OnHold | Deleted | Denied | Approved | Published | Draft // 111111
}
