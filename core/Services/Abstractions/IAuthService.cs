namespace api.core.services.abstractions;

public interface IAuthService
{
    string SignUp(string email, string password);
}
