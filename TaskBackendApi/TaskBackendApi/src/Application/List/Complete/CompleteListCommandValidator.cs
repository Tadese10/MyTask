using FluentValidation;

namespace Application.List.Complete;

internal sealed class CompleteListCommandValidator : AbstractValidator<CompleteListCommand>
{
    public CompleteListCommandValidator() 
    {
        RuleFor(c => c.ListItemId).NotEmpty();
    }
}
