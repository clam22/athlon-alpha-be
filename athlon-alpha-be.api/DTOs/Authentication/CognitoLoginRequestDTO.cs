using System.ComponentModel.DataAnnotations;

namespace athlon_alpha_be.api.DTOs.Authentication;

public record CognitoLoginRequestDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}
