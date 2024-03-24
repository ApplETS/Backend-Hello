
namespace api.emails.Models;

public class UserDeactivationModel : LayoutEmailBaseModel
{
    /// <summary>
    /// Example: "Dear ACCOUNT_NAME,"
    /// </summary>
    public required string Salutation { get; set; }

    /// <summary>
    /// Example "Your account has been deactivated for the following reason :"
    /// </summary>
    public required string UserDeactivationHeader { get; set; }

    /// <summary>
    /// Reason of the user's deactivation
    /// </summary>
    public required string UserDeactivationReason { get; set; }
}
