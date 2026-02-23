using System.ComponentModel.DataAnnotations;

namespace athlon_alpha_be.api.DTOs.Authentication;

public class CognitoRegisterRequestDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Surname { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
