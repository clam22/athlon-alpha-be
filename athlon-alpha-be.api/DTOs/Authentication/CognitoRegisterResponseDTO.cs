namespace athlon_alpha_be.api.DTOs.Authentication;

public record CognitoRegisterResponseDTO
{
    public string CognitoSub { get; set; } = string.Empty;
}
