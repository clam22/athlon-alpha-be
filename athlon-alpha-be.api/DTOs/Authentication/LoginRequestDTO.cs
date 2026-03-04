namespace athlon_alpha_be.api.DTOs.Authentication;

public record LoginRequestDTO
{
    public required string Email { get; set; } = string.Empty;
    public required string Password { get; set; } = string.Empty;
}
