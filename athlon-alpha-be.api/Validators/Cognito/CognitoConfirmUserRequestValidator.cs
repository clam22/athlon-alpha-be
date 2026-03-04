using athlon_alpha_be.api.DTOs.Cognito;

using FluentValidation;

namespace athlon_alpha_be.api.Validators.Cognito;

public class CognitoConfirmUserRequestValidator : AbstractValidator<CognitoConfirmUserRequestDTO>
{
    public CognitoConfirmUserRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.ConfirmationCode).NotEmpty();
    }
}
