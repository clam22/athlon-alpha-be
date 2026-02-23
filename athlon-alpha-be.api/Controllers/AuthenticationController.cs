using athlon_alpha_be.api.DTOs.Authentication;
using athlon_alpha_be.api.DTOs.User;
using athlon_alpha_be.api.Services;

using Microsoft.AspNetCore.Mvc;

namespace athlon_alpha_be.api.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController(ICognitoService _cognitoService, IUserService _userService) : ControllerBase
{
    [HttpPost("signin")]
    public async Task<ActionResult<LoginResponseDTO>> SignInAsync([FromBody] LoginRequestDTO loginRequest)
    {
        CognitoLoginRequestDTO cognitoLoginRequest = new CognitoLoginRequestDTO
        {
            Email = loginRequest.Email,
            Password = loginRequest.Password
        };

        CognitoLoginResponseDTO? result = await _cognitoService.LoginUserAsync(cognitoLoginRequest);

        if (result == null)
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(new LoginResponseDTO
        {
            AccessToken = result.AccessToken,
            RefreshToken = result.RefreshToken,
            IdToken = result.IdToken,
            TokenType = result.TokenType,
            ExpiresIn = result.ExpiresIn
        });
    }

    [HttpPost("signup")]
    public async Task<ActionResult> SignUpAsync([FromBody] RegisterRequestDTO registerRequest)
    {
        CognitoRegisterRequestDTO cognitoRegisterRequest = new CognitoRegisterRequestDTO
        {
            Name = registerRequest.Name,
            Surname = registerRequest.Surname,
            Email = registerRequest.Email,
            Password = registerRequest.Password
        };

        CognitoRegisterResponseDTO cognitoRegisterResponse = await _cognitoService.RegisterUserAsync(cognitoRegisterRequest);

        CreateUserRequestDTO createUserRequest = new CreateUserRequestDTO()
        {
            Name = registerRequest.Name,
            Surname = registerRequest.Surname,
            Email = registerRequest.Email,
            CognitoSub = cognitoRegisterResponse.CognitoSub
        };

        await _userService.CreateUserAsync(createUserRequest);

        return NoContent();
    }
}


