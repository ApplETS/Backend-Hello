using api.core.services.abstractions;

using Supabase;

namespace api.core.Services;

public class AuthService(Client client) : IAuthService
{
    public string SignUp(string email, string password)
    {
        var user = client.Auth.SignUp(email, password).Result;

        return user?.User?.Id 
            ?? throw new Exception("An error occured while signing up the user");
    }
}
