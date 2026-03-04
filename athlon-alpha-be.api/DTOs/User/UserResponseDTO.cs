namespace athlon_alpha_be.api.DTOs.User;

public record UserResponseDTO
{
    public Guid Id { get; set; }
    public string CognitoSub { get; set; } = string.Empty;
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset LastModified { get; set; }
}
