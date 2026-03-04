using athlon_alpha_be.api.DTOs.Authentication;

using FluentValidation;

namespace athlon_alpha_be.api.Validators.Authentication;

public class ConfirmUserRequestValidator : AbstractValidator<ConfirmUserRequestDTO>
{
    public ConfirmUserRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.ConfirmationCode).NotEmpty().EmailAddress();
    }
}
