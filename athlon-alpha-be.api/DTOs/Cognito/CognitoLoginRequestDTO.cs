namespace athlon_alpha_be.api.DTOs.Cognito;

public record CognitoLoginRequestDTO
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
