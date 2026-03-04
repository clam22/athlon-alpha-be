namespace athlon_alpha_be.api.DTOs.Authentication;

public class ConfirmUserRequestDTO
{
    public string Email { get; set; } = string.Empty;
    public string ConfirmationCode { get; set; } = string.Empty;
}
