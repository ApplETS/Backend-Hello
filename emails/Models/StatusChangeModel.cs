
namespace api.emails.Models;

public class StatusChangeModel : LayoutEmailBaseModel
{
    /// <summary>
    /// Example: "Dear ACCOUNT_NAME,"
    /// </summary>
    public required string Salutation { get; set; }

    /// <summary>
    /// Example "The post « POST_NAME » was placed in the status of "
    /// </summary>
    public required string StatusHeaderText { get; set; }

    /// <summary>
    /// Example: "refused"
    /// </summary>
    public required string StatusNameText { get; set; }

    /// <summary>
    /// if null should not render the refusal reason
    /// </summary>
    public string? StatusRefusalReason { get; set; }

    /// <summary>
    /// Example "Reason for refusal :"
    /// </summary>
    public required string StatusRefusalHeader { get; set; }


    /// <summary>
    /// Example "See publication"
    /// </summary>
    public required string ButtonSeePublicationText { get; set; }

    public required Uri ButtonLink { get; set; }
}
