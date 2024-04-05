namespace api.emails.Models;

public class ReportModel : LayoutEmailBaseModel
{
    /// <summary>
    /// Example: "Dear MODERATOR_NAME,"
    /// </summary>
    public required string Salutation { get; set; }

    /// <summary>
    /// Example: "Event Reports Alert"
    /// </summary>
    public required string AlertSubject { get; set; }

    /// <summary>
    /// Example: "The following event has received multiple reports:"
    /// </summary>
    public required string AlertMessage { get; set; }

    /// <summary>
    /// Example: "Event Title: "
    /// </summary>
    public required string EventTitleHeader { get; set; }

    public required string EventTitle { get; set; }

    /// <summary>
    /// Example: "Number of Reports: "
    /// </summary>
    public required string NumberOfReportsHeader { get; set; }

    public required int NumberOfReports { get; set; }

    /// <summary>
    /// Example: "Please take necessary action."
    /// </summary>
    public required string ActionRequiredMessage { get; set; }

    /// <summary>
    /// Example: "View Event"
    /// </summary>
    public required string ViewEventButtonText { get; set; }

    public required Uri EventLink { get; set; }
}
