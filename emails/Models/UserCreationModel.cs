namespace api.emails.Models;

public class UserCreationModel : LayoutEmailBaseModel
{
    /// <summary>
    /// Example: "Dear ACCOUNT_NAME,"
    /// </summary>
    public required string Salutation { get; set; }

    /// <summary>
    /// Example "Your account has been created!"
    /// </summary>
    public required string AccountCreatedText { get; set; }

    /// <summary>
    /// Example "Your temporary password is: "
    /// </summary>
    public required string TemporaryPasswordHeader { get; set; }


    public required string TemporaryPassword { get; set; }

    /// <summary>
    /// Example "Login"
    /// </summary>
    public required string LoginButtonText { get; set; }

    public required Uri ButtonLink { get; set; }
}
