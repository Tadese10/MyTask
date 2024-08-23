using Application.Taskss.Update;
using FluentValidation;

namespace Application.Taskss.Create;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Priority).IsInEnum();
        RuleFor(c => c.Description).NotEmpty().MaximumLength(255);
        RuleFor(c => c.StartDate).GreaterThanOrEqualTo(DateTime.Today).When(x => x.StartDate.HasValue);
        RuleFor(c => c.EndDate).GreaterThanOrEqualTo(DateTime.Today).GreaterThan(d=>d.StartDate).When(x => x.EndDate.HasValue);
        RuleFor(c => c.GroupId).NotEmpty();
        RuleFor(c => c.ListId).NotEmpty();
    }
}
