namespace api.emails;
public static class EmailsUtils
{
    public const string StatusChangeTemplate = "StatusChange.cshtml";

    public const string UserCreationTemplate = "UserSignUp.cshtml";

    public const string ReportTemplate = "Report.cshtml";

    public const string UserDeactivationTemplate = "UserDeactivation.cshtml";

    internal static readonly string TemplateEmbeddedResourceNamespace = "api.emails.Views.";
}
