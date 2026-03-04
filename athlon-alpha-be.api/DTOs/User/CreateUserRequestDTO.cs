namespace athlon_alpha_be.api.DTOs.User;

public record CreateUserRequestDTO
{
    public string CognitoSub { get; set; } = string.Empty;
}
