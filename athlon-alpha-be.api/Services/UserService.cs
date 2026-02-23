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
        if (await _appDbContext.Users.AsNoTracking().AnyAsync(u => u.Email == createUserRequest.Email))
        {
            throw new ConflictException("User already exists");
        }

        var user = new User
        {
            Name = createUserRequest.Name,
            Surname = createUserRequest.Surname,
            Email = createUserRequest.Email,
            CognitoSub = createUserRequest.CognitoSub
        };

        await _appDbContext.AddAsync(user);
        await _appDbContext.SaveChangesAsync();

        _logger.LogInformation("User created successfully. UserID: {Id}, Email: {Email}", user.Id, user.Email);

        return new UserResponseDTO
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
        };
    }

    public async Task<IEnumerable<UserResponseDTO>> GetUsersAsync()
    {
        return await _appDbContext.Users
            .AsNoTracking()
            .Select(user => new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email
            })
            .ToListAsync();
    }

    public async Task<UserResponseDTO> GetUserByIdAsync(Guid id)
    {
        User? user = await _appDbContext.Users.FindAsync(id) ?? throw new NotFoundException("User not found");

        return new UserResponseDTO
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email
        };
    }

    public async Task<UserResponseDTO> GetUserByEmailAsync(string email)
    {
        User? user = await _appDbContext.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(user => user.Email == email) ?? throw new NotFoundException("User not found");

        return new UserResponseDTO
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email
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
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email
        };
    }

    public async Task UpdateUserAsync(UpdateUserRequestDTO updateUserRequest)
    {
        User? user = await _appDbContext.Users.FindAsync(updateUserRequest.Id) ?? throw new NotFoundException("User not found");

        user.Name = updateUserRequest.Name;
        user.Surname = updateUserRequest.Surname;

        await _appDbContext.SaveChangesAsync();

        _logger.LogInformation("User updated successfully. UserID: {Id}, Email: {Email}", user.Id, user.Email);
    }

    public async Task DeleteUserAsync(Guid id)
    {
        User? user = await _appDbContext.Users.FindAsync(id) ?? throw new NotFoundException("User not found");

        _appDbContext.Users.Remove(user);
        await _appDbContext.SaveChangesAsync();
        _logger.LogInformation("User updated successfully. UserID: {Id}, Email: {Email}", user.Id, user.Email);
    }
}
