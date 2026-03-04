namespace athlon_alpha_be.api.DTOs.Cognito;

public record CognitoRegisterResponseDTO
{
    public string CognitoSub { get; set; } = string.Empty;
    public string Session { get; set; } = string.Empty;
    public bool? UserConfirmed { get; set; } = false;
}
