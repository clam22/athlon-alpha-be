namespace athlon_alpha_be.api.DTOs.Authentication;

public class RegisterResponseDTO
{
    public string Session { get; set; } = string.Empty;
    public bool? UserConfirmed { get; set; } = false;
}
