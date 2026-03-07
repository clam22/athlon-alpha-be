using athlon_alpha_be.api.DTOs.User;
using athlon_alpha_be.api.Exceptions;
using athlon_alpha_be.database.Models;
using athlon_alpha_be.database.Persistence;

using Microsoft.EntityFrameworkCore;

namespace athlon_alpha_be.api.Services;

public class UserService(AppDbContext _appDbContext, ILogger<UserService> _logger) : IUserService
{
    public async Task<UserResponseDTO> CreateUserAsync(CreateUserRequestDTO createUserRequest)
    {
        if (await _appDbContext.Users.AsNoTracking().AnyAsync(u => u.CognitoSub == createUserRequest.CognitoSub))
        {
            throw new ConflictException("User already exists");
        }

        var user = new User
        {
            CognitoSub = createUserRequest.CognitoSub
        };

        await _appDbContext.AddAsync(user);
        await _appDbContext.SaveChangesAsync();

        _logger.LogInformation("User created successfully. UserID: {Id}, CognitoSub: {Sub}", user.Id, user.CognitoSub);

        return new UserResponseDTO
        {
            Id = user.Id,
            CognitoSub = user.CognitoSub,
            Created = user.Created,
            LastModified = user.LastModified
        };
    }

    public async Task<IEnumerable<UserResponseDTO>> GetUsersAsync()
    {
        return await _appDbContext.Users
            .AsNoTracking()
            .Select(user => new UserResponseDTO
            {
                Id = user.Id,
                CognitoSub = user.CognitoSub,
                Created = user.Created,
                LastModified = user.LastModified,
            })
            .ToListAsync();
    }

    public async Task<UserResponseDTO> GetUserByIdAsync(Guid id)
    {
        User? user = await _appDbContext.Users.FindAsync(id) ?? throw new NotFoundException("User not found");

        return new UserResponseDTO
        {
            Id = user.Id,
            CognitoSub = user.CognitoSub,
            Created = user.Created,
            LastModified = user.LastModified
        };
    }

    public async Task<UserResponseDTO> GetUserByCognitoSub(string cognitoSub)
    {
        User? user = await _appDbContext.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(user => user.CognitoSub == cognitoSub) ?? throw new NotFoundException("User not found");

        return new UserResponseDTO
        {
            Id = user.Id,
            CognitoSub = user.CognitoSub,
            Created = user.Created,
            LastModified = user.LastModified
        };
    }

    public async Task DeleteUserAsync(Guid id)
    {
        User? user = await _appDbContext.Users.FindAsync(id) ?? throw new NotFoundException("User not found");

        _appDbContext.Users.Remove(user);
        await _appDbContext.SaveChangesAsync();
        _logger.LogInformation("User updated successfully. UserID: {Id}", user.Id);
    }
}
