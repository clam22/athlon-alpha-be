using athlon_alpha_be.api.DTOs.Cognito;

namespace athlon_alpha_be.api.Services;

public interface ICognitoService
{
    Task<CognitoLoginResponseDTO?> LoginUserAsync(CognitoLoginRequestDTO loginRequest);
    Task<CognitoRegisterResponseDTO> RegisterUserAsync(CognitoRegisterRequestDTO registerRequest);
    Task DeleteUserAsync(string username);
    Task ConfirmUserAsync(CognitoConfirmUserRequestDTO cognitoConfirmUserRequest);
    Task<CognitoGetUserResponseDTO> GetUserAsync(string accessToken);
}
