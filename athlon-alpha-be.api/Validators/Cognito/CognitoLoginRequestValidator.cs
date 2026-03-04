using athlon_alpha_be.api.DTOs.Cognito;

using FluentValidation;

namespace athlon_alpha_be.api.Validators.Cognito;

public class CognitoLoginRequestValidator : AbstractValidator<CognitoLoginRequestDTO>
{
    public CognitoLoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
    }
}
