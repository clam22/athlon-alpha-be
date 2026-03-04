namespace athlon_alpha_be.api.DTOs.Authentication;

public record RegisterRequestDTO
{
    public required string Name { get; set; } = string.Empty;
    public required string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

}
