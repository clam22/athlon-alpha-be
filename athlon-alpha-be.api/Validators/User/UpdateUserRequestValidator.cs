using athlon_alpha_be.api.DTOs.User;

using FluentValidation;

namespace athlon_alpha_be.api.Validators.User;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequestDTO>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Surname).NotEmpty();
    }
}
