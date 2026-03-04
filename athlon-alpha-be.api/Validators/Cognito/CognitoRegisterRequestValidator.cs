using athlon_alpha_be.api.DTOs.Cognito;

using FluentValidation;

namespace athlon_alpha_be.api.Validators.Cognito;

public class CognitoRegisterRequestValidator : AbstractValidator<CognitoRegisterRequestDTO>
{
    public CognitoRegisterRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Surname).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
    }
}
