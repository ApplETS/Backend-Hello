using System.Text.Json.Serialization;

namespace api.core.Data.Enums;

/// <summary>
/// 1 - InappropriateContent
/// 2 - FalseInformation
/// 3 - AbusiveBehavior
/// 4 - ObsoleteInformation
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ReportCategory
{
    InappropriateContent = 1,
    FalseInformation = 2,
    AbusiveBehavior = 3,
    ObsoleteInformation = 4
}
