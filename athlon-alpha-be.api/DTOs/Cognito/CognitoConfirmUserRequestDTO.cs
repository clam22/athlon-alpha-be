namespace athlon_alpha_be.api.DTOs.Cognito;

public class CognitoConfirmUserRequestDTO
{
    public string Email { get; set; } = string.Empty;
    public string ConfirmationCode { get; set; } = string.Empty;
}
