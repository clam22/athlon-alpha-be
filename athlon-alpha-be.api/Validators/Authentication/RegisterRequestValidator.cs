using athlon_alpha_be.api.DTOs.Authentication;

using FluentValidation;

namespace athlon_alpha_be.api.Validators.Authentication;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestDTO>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Surname).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
