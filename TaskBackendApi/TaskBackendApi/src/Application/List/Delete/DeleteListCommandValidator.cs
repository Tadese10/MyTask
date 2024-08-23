using FluentValidation;

namespace Application.List.Delete;

internal sealed class DeleteListCommandValidator : AbstractValidator<DeleteListCommand>
{
    public DeleteListCommandValidator()
    {
        RuleFor(c => c.ListItemId).NotEmpty();
    }
}
