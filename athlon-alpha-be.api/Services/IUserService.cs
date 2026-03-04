using athlon_alpha_be.api.DTOs.User;

namespace athlon_alpha_be.api.Services;

public interface IUserService
{
    Task<UserResponseDTO> CreateUserAsync(CreateUserRequestDTO createUserRequest);
    Task<IEnumerable<UserResponseDTO>> GetUsersAsync();
    Task<UserResponseDTO> GetUserByIdAsync(Guid id);
    Task<UserResponseDTO> GetUserByCognitoSub(string cognitoSub);
    Task DeleteUserAsync(Guid id);
}
