using System.ComponentModel.DataAnnotations;

namespace athlon_alpha_be.api.DTOs.User;

public record CreateUserRequestDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Surname { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string CognitoSub { get; set; } = string.Empty;
}
