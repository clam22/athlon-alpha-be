using athlon_alpha_be.api.DTOs.Authentication;

namespace athlon_alpha_be.api.Services;

public interface ICognitoService
{
    Task<CognitoLoginResponseDTO?> LoginUserAsync(CognitoLoginRequestDTO loginRequest);
    Task<CognitoRegisterResponseDTO> RegisterUserAsync(CognitoRegisterRequestDTO registerRequest);
}
