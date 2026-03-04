using athlon_alpha_be.api.DTOs.User;

using FluentValidation;

namespace athlon_alpha_be.api.Validators.User;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequestDTO>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.CognitoSub).NotEmpty();
    }
}
