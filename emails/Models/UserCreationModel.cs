using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.emails.Models;

public class UserCreationModel : LayoutEmailBaseModel
{
    /// <summary>
    /// Example: "Dear ACCOUNT_NAME,"
    /// </summary>
    public string Salutation { get; set; }

    /// <summary>
    /// Example "Your account has been created!"
    /// </summary>
    public string AccountCreatedText { get; set; }

    /// <summary>
    /// Example "Your temporary password is: "
    /// </summary>
    public string TemporaryPasswordHeader { get; set; }


    public string TemporaryPassword { get; set; }

    /// <summary>
    /// Example "Login"
    /// </summary>
    public string LoginButtonText { get; set; }

    public Uri ButtonLink { get; set; }
}
