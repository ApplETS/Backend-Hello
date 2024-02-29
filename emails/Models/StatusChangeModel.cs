
namespace api.emails.Models;

public class StatusChangeModel : LayoutEmailBaseModel
{
    /// <summary>
    /// Example: "Dear ACCOUNT_NAME,"
    /// </summary>
    public string Salutation { get; set; }

    /// <summary>
    /// Example "The post « POST_NAME » was placed in the status of "
    /// </summary>
    public string StatusHeaderText { get; set; }

    /// <summary>
    /// Example: "refused"
    /// </summary>
    public string StatusNameText { get; set; }

    /// <summary>
    /// if null should not render the refusal reason
    /// </summary>
    public string? StatusRefusalReason { get; set; }

    /// <summary>
    /// Example "Reason for refusal :"
    /// </summary>
    public string StatusRefusalHeader { get; set; }


    /// <summary>
    /// Example "See publication"
    /// </summary>
    public string ButtonSeePublicationText { get; set; }

    public Uri ButtonLink { get; set; }
}
