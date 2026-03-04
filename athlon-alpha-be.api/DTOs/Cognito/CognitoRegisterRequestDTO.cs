namespace athlon_alpha_be.api.DTOs.Cognito;

public record CognitoRegisterRequestDTO
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
