using FluentValidation;

namespace Application.Group.Create;

public class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
{
    public CreateGroupCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.GroupType).IsInEnum();
        RuleFor(c => c.Name).NotEmpty().MaximumLength(30);
    }
}
