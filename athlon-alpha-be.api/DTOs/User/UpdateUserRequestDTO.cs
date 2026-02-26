namespace athlon_alpha_be.api.DTOs.User;

public record UpdateUserRequestDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
}
