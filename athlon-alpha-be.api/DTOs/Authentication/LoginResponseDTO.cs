namespace athlon_alpha_be.api.DTOs.Authentication;

public record LoginResponseDTO
{
    public required string CognitoId { get; set; } = string.Empty;
    public required string Name { get; set; } = string.Empty;
    public required string Surname { get; set; } = string.Empty;
    public required string Email { get; set; } = string.Empty;
}
