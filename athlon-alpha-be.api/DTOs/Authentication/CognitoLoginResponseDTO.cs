namespace athlon_alpha_be.api.DTOs.Authentication;

public record CognitoLoginResponseDTO
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string IdToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = string.Empty;
    public int? ExpiresIn { get; set; }
}
