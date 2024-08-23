using FluentValidation;

namespace Application.List.Create;

public class CreateListCommandValidator : AbstractValidator<CreateListCommand>
{
    public CreateListCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.ListType).IsInEnum();
        RuleFor(c => c.Description).NotEmpty().MaximumLength(255);
    }
}
