using FluentValidation;

namespace Application.Taskss.Create;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(c => c.Priority).IsInEnum();
        RuleFor(c => c.Description).NotEmpty().MaximumLength(255);
        RuleFor(c => c.StartDate).GreaterThanOrEqualTo(DateTime.Today).When(x => x.StartDate.HasValue);
        RuleFor(c => c.EndDate).GreaterThanOrEqualTo(d=>d.StartDate).When(x => x.EndDate.HasValue);
        RuleFor(c => c.GroupId).NotEmpty();
        RuleFor(c => c.ListId).NotEmpty();
    }
}
