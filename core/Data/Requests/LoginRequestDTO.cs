namespace api.core.Data.Requests;

public class LoginRequestDTO
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
