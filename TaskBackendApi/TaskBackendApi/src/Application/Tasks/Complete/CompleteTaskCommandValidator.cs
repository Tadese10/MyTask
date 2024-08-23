using FluentValidation;

namespace Application.Tasks.Complete;

internal sealed class CompleteTaskCommandValidator : AbstractValidator<Taskss.Complete.CompleteTaskCommand>
{
    public CompleteTaskCommandValidator()
    {
        RuleFor(c => c.TasksItemId).NotEmpty();
    }
}
