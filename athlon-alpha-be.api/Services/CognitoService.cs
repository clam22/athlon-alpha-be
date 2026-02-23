using System.Security.Cryptography;
using System.Text;

using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;

using athlon_alpha_be.api.Configuration;
using athlon_alpha_be.api.DTOs.Authentication;

using Microsoft.Extensions.Options;

namespace athlon_alpha_be.api.Services;

public class CognitoService : ICognitoService
{
    private readonly AmazonCognitoIdentityProviderClient _client;
    private readonly CognitoSettings _settings;

    public CognitoService(IOptions<CognitoSettings> settings)
    {
        _settings = settings.Value;
        _client = new AmazonCognitoIdentityProviderClient(RegionEndpoint.GetBySystemName(_settings.Region));
    }
    public async Task<CognitoLoginResponseDTO?> LoginUserAsync(CognitoLoginRequestDTO loginRequest)
    {
        InitiateAuthRequest authResult = new InitiateAuthRequest
        {
            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
            ClientId = _settings.ClientId,
            AuthParameters = new Dictionary<string, string>
            {
                { "USERNAME", loginRequest.Email },
                { "PASSWORD", loginRequest.Password },
                { "SECRET_HASH", CalculateSecretHash(loginRequest.Email) }
            }
        };

        InitiateAuthResponse response = await _client.InitiateAuthAsync(authResult);

        return new CognitoLoginResponseDTO
        {
            AccessToken = response.AuthenticationResult.AccessToken,
            RefreshToken = response.AuthenticationResult.RefreshToken,
            IdToken = response.AuthenticationResult.IdToken,
            ExpiresIn = response.AuthenticationResult.ExpiresIn,
            TokenType = response.AuthenticationResult.TokenType
        };
    }

    public async Task<CognitoRegisterResponseDTO> RegisterUserAsync(CognitoRegisterRequestDTO registerRequest)
    {
        SignUpRequest signUpRequest = new SignUpRequest
        {
            ClientId = _settings.ClientId,
            Username = registerRequest.Email,
            Password = registerRequest.Password,
            SecretHash = CalculateSecretHash(registerRequest.Email),
            UserAttributes = new List<AttributeType>
            {
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
            }
        };

        SignUpResponse response = await _client.SignUpAsync(signUpRequest);

        await _client.AdminAddUserToGroupAsync(new AdminAddUserToGroupRequest
        {
            UserPoolId = _settings.UserPoolId,
            Username = registerRequest.Email,
            GroupName = "User"
        });

        return new CognitoRegisterResponseDTO
        {
            CognitoSub = response.UserSub
        };
    }

    private string CalculateSecretHash(string email)
    {
        var key = Encoding.UTF8.GetBytes(_settings.ClientSecret);
        using var hmac = new HMACSHA256(key);
        var message = Encoding.UTF8.GetBytes(email + _settings.ClientId);
        var hash = hmac.ComputeHash(message);
        return Convert.ToBase64String(hash);
    }
}
