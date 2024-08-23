using FluentValidation;

namespace Application.Group.Complete;

internal sealed class CompleteGroupCommandValidator : AbstractValidator<CompleteGroupCommand>
{
    public CompleteGroupCommandValidator() 
    {
        RuleFor(c => c.GroupItemId).NotEmpty();
    }
}
