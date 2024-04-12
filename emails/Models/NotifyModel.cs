
namespace api.emails.Models;

public class NotifyModel : LayoutEmailBaseModel
{
    /// <summary>
    /// Example: "Hello!"
    /// </summary>
    public required string Salutation { get; set; }

    /// <summary>
    /// Example "You have a new publication that you may be interested in"
    /// </summary>
    public required string HeaderText { get; set; }

    /// <summary>
    /// Publication Title
    /// </summary>
    public required string PublicationTitle { get; set; }

    /// <summary>
    /// Example "See publication"
    /// </summary>
    public required string ButtonSeePublicationText { get; set; }

    public required Uri ButtonLink { get; set; }

    public required string UnsubscribeHeaderText { get; set; }

    public required string UnsubscribeLinkText { get; set; }

    public required Uri UnsubscribeLink { get; set; }
}
