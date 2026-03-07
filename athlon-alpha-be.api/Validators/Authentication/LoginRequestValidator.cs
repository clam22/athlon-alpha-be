using athlon_alpha_be.api.DTOs.Authentication;

using FluentValidation;

namespace athlon_alpha_be.api.Validators.Authentication;

public class LoginRequestValidator : AbstractValidator<LoginRequestDTO>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
    }
}
