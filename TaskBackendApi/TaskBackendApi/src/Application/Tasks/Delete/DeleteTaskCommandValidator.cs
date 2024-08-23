using FluentValidation;

namespace Application.Taskss.Delete;

internal sealed class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
{
    public DeleteTaskCommandValidator()
    {
        RuleFor(c => c.TasksItemId).NotEmpty();
    }
}
