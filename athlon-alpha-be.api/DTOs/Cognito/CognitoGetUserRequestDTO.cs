namespace athlon_alpha_be.api.DTOs.Cognito;

public record CognitoGetUserRequestDTO
{
    public string AccessToken { get; set; } = string.Empty;
}
