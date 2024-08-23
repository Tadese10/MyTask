using FluentValidation;

namespace Application.Group.Delete;

internal sealed class DeleteGroupCommandValidator : AbstractValidator<DeleteGroupCommand>
{
    public DeleteGroupCommandValidator()
    {
        RuleFor(c => c.GroupItemId).NotEmpty();
    }
}
