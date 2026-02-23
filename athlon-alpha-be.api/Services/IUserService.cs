using athlon_alpha_be.api.DTOs.User;

namespace athlon_alpha_be.api.Services;

public interface IUserService
{
    Task<UserResponseDTO> CreateUserAsync(CreateUserRequestDTO user);
    Task<IEnumerable<UserResponseDTO>> GetUsersAsync();
    Task<UserResponseDTO> GetUserAsync(Guid id);
    Task<UserResponseDTO> GetUserAsync(string email);
    Task<UserResponseDTO> UpdateUserAsync(UpdateUserRequestDTO user);
    Task DeleteUserAsync(Guid id);
}
