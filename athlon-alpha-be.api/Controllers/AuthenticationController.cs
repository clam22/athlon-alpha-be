using athlon_alpha_be.api.DTOs.Authentication;
using athlon_alpha_be.api.DTOs.User;
using athlon_alpha_be.api.DTOs.Cognito;
using athlon_alpha_be.api.Services;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.Results;

using System.IdentityModel.Tokens.Jwt;

namespace athlon_alpha_be.api.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController(
    ICognitoService _cognitoService,
    IUserService _userService,
    ILogger<AuthenticationController> _logger,
    IValidator<LoginRequestDTO> _loginRequestValidator,
    IValidator<RegisterRequestDTO> _registerRequestValidator
    ) : ControllerBase
{
    [HttpPost("signin")]
    public async Task<ActionResult<LoginResponseDTO>> SignInAsync([FromBody] LoginRequestDTO loginRequest)
    {
        ValidationResult loginRequestValidationResult = await _loginRequestValidator.ValidateAsync(loginRequest);

        if (!loginRequestValidationResult.IsValid)
        {
            _logger.LogWarning("Login validation failed for email {Email}. Errors: {@Errors}", loginRequest.Email, loginRequestValidationResult.Errors);
            return BadRequest("Invalid login request");
        }

        CognitoLoginResponseDTO? result = await _cognitoService.LoginUserAsync(new CognitoLoginRequestDTO
        {
            Email = loginRequest.Email,
            Password = loginRequest.Password
        });

        if (result == null)
        {
            _logger.LogWarning("Authentication failed for email {Email}. Invalid credentials.", loginRequest.Email);
            return Unauthorized("Invalid credentials");
        }

        SetTokenCookies(result.AccessToken, result.RefreshToken, result.IdToken);

        Dictionary<string, string> claims = ReadJwtClaims(result.IdToken);

        foreach (var claim in claims)
        {
            Console.WriteLine($"Claim Type: {claim.Key}, Claim Value: {claim.Value}");
        }
       
        return Ok(new LoginResponseDTO
        {
            CognitoId = claims["sub"],
            Email = claims["email"],
            Name = claims["given_name"],
            Surname = claims["family_name"]
        });
    }

    [HttpPost("signup")]
    public async Task<ActionResult<RegisterResponseDTO>> RegisterUserAsync([FromBody] RegisterRequestDTO registerRequest)
    {
        ValidationResult registerRequestValidationResult = await _registerRequestValidator.ValidateAsync(registerRequest);

        if (!registerRequestValidationResult.IsValid)
        {
            _logger.LogWarning("Registration validation failed for email {Email}. Errors: {@Errors}", registerRequest.Email, registerRequestValidationResult.Errors);
            return BadRequest("Invalid credentials");
        }

        CognitoRegisterResponseDTO? cognitoRegisterResponse = null;

        try
        {
            cognitoRegisterResponse = await _cognitoService.RegisterUserAsync(new CognitoRegisterRequestDTO
            {
                Name = registerRequest.Name,
                Surname = registerRequest.Surname,
                Email = registerRequest.Email,
                Password = registerRequest.Password
            });

            await _userService.CreateUserAsync(new CreateUserRequestDTO()
            {
                CognitoSub = cognitoRegisterResponse.CognitoSub
            });

            return new RegisterResponseDTO
            {
                Session = cognitoRegisterResponse.Session,
                UserConfirmed = cognitoRegisterResponse.UserConfirmed
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "User registration orchestration failed for email {Email}", registerRequest.Email);

            if (cognitoRegisterResponse is not null)
            {
                _logger.LogWarning("Initiating compensating action. Deleting Cognito user for email {Email}.", registerRequest.Email);
                await _cognitoService.DeleteUserAsync(registerRequest.Email);
            }
            
            throw;
        }
    }

    [HttpPost("confirm-signup")]
    public async Task <ActionResult> ConfirmUserAsync([FromBody] ConfirmUserRequestDTO confirmUserRequest)
    {
        await _cognitoService.ConfirmUserAsync(new CognitoConfirmUserRequestDTO
        {
            Email = confirmUserRequest.Email,
            ConfirmationCode = confirmUserRequest.ConfirmationCode,
        });

        _logger.LogInformation("User with email {Email} successfully confirmed their account.", confirmUserRequest.Email);
        return NoContent();
    }

    private void SetTokenCookies(string accessToken, string refreshToken, string idToken)
    {
        bool isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";

        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = isProduction, 
            SameSite = SameSiteMode.Lax, 
            Expires = DateTime.UtcNow.AddMinutes(60)
        };

        Response.Cookies.Append("accessToken", accessToken, options);
        Response.Cookies.Append("refreshToken", refreshToken, options);
        Response.Cookies.Append("idToken", idToken, options);
    }

    private Dictionary<string, string> ReadJwtClaims(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        return jwt.Claims
        .GroupBy(c => c.Type)
        .ToDictionary(g => g.Key, g => g.First().Value);
    }
}


