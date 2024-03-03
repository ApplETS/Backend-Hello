namespace api.emails;
public static class EmailsUtils
{
    public const string StatusChangeTemplate = "StatusChange.cshtml";

    public const string UserCreationTemplate = "UserSignUp.cshtml";

    internal static readonly string TemplateEmbeddedResourceNamespace = "api.emails.Views.";
}
