
using System.Security.Cryptography;
using System.Text;

using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;

using athlon_alpha_be.api.Configuration;
using athlon_alpha_be.api.DTOs.Cognito;

using Microsoft.Extensions.Options;

namespace athlon_alpha_be.api.Services;

public class CognitoService : ICognitoService
{
    private readonly AmazonCognitoIdentityProviderClient _client;
    private readonly CognitoSettings _settings;
    private readonly ILogger<CognitoService> _logger;

    public CognitoService(IOptions<CognitoSettings> settings, ILogger<CognitoService> logger)
    {
        _settings = settings.Value;
        _client = new AmazonCognitoIdentityProviderClient(RegionEndpoint.GetBySystemName(_settings.Region));
        _logger = logger;
    }
    public async Task<CognitoLoginResponseDTO?> LoginUserAsync(CognitoLoginRequestDTO loginRequest)
    {
        InitiateAuthResponse response = await _client.InitiateAuthAsync(new InitiateAuthRequest
        {
            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
            ClientId = _settings.ClientId,
            AuthParameters = new Dictionary<string, string>
            {
                { "USERNAME", loginRequest.Email },
                { "PASSWORD", loginRequest.Password },
                { "SECRET_HASH", CalculateSecretHash(loginRequest.Email) }
            }
        });

        _logger.LogInformation("Cognito login successful for user: {Email}", loginRequest.Email);
        return new CognitoLoginResponseDTO
        {
            AccessToken = response.AuthenticationResult.AccessToken,
            RefreshToken = response.AuthenticationResult.RefreshToken,
            IdToken = response.AuthenticationResult.IdToken,
            ExpiresIn = response.AuthenticationResult.ExpiresIn,
            TokenType = response.AuthenticationResult.TokenType,
            Session = response.Session
        };
    }

    public async Task<CognitoRegisterResponseDTO> RegisterUserAsync(CognitoRegisterRequestDTO registerRequest)
    {
        SignUpResponse response = await _client.SignUpAsync(new SignUpRequest
        {
            ClientId = _settings.ClientId,
            Username = registerRequest.Email,
            Password = registerRequest.Password,
            SecretHash = CalculateSecretHash(registerRequest.Email),
            UserAttributes = [
                new AttributeType
                {
                    Name = "email",
                    Value = registerRequest.Email
                },
                new AttributeType
                {
                    Name = "given_name",
                    Value = registerRequest.Name
                },
                new AttributeType
                {
                    Name = "family_name",
                    Value = registerRequest.Surname
                }
            ]
        });

        _logger.LogInformation("Cognito registration successful for user: {Email}", registerRequest.Email);

        await _client.AdminAddUserToGroupAsync(new AdminAddUserToGroupRequest
        {
            UserPoolId = _settings.UserPoolId,
            Username = registerRequest.Email,
            GroupName = "User"
        });

        return new CognitoRegisterResponseDTO
        {
            CognitoSub = response.UserSub,
            Session = response.Session,
            UserConfirmed = response.UserConfirmed
        };
    }

    public async Task DeleteUserAsync(string email)
    {
        await _client.AdminDeleteUserAsync(new AdminDeleteUserRequest
        {
            Username = email,
            UserPoolId = _settings.UserPoolId,
        });
    }

    public async Task ConfirmUserAsync(CognitoConfirmUserRequestDTO cognitoConfirmUserRequest)
    {
        await _client.ConfirmSignUpAsync(new ConfirmSignUpRequest
        {
            Username = cognitoConfirmUserRequest.Email,
            ClientId = _settings.ClientId,
            ConfirmationCode = cognitoConfirmUserRequest.ConfirmationCode,
            SecretHash = CalculateSecretHash(cognitoConfirmUserRequest.Email)
        });

        _logger.LogInformation("Cognito user confirmation successful for user: {Email}", cognitoConfirmUserRequest.Email);
    }

    public async Task<CognitoGetUserResponseDTO> GetUserAsync(string accessToken)
    {
        GetUserResponse response = await _client.GetUserAsync(new GetUserRequest
        {
            AccessToken = accessToken
        });

        return new CognitoGetUserResponseDTO
        {
            Name = response.UserAttributes.FirstOrDefault(a => a.Name == "given_name")?.Value ?? "",
            Surname = response.UserAttributes.FirstOrDefault(a => a.Name == "family_name")?.Value ?? "",
            Email = response.UserAttributes.FirstOrDefault(a => a.Name == "email")?.Value ?? "",
        };
    }

    private string CalculateSecretHash(string email)
    {
        byte[] key = Encoding.UTF8.GetBytes(_settings.ClientSecret);
        using var hmac = new HMACSHA256(key);
        byte[] message = Encoding.UTF8.GetBytes(email + _settings.ClientId);
        byte[] hash = hmac.ComputeHash(message);
        return Convert.ToBase64String(hash);
    }
}
