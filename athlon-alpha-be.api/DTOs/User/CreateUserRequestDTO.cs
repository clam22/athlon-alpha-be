namespace athlon_alpha_be.api.DTOs.User;

public class CreateUserRequestDTO
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
