using FluentValidation;
using ProjectManagement.Application.Commands.Tasks;

namespace ProjectManagement.Application.Validators.Tasks;

public class UpdateTaskValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Task ID must be greater than 0");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.Priority)
            .InclusiveBetween(1, 3).WithMessage("Priority must be between 1 and 3");

        RuleFor(x => x.Status)
            .InclusiveBetween(1, 5).WithMessage("Status must be between 1 and 5");

        RuleFor(x => x.DueDate)
            .NotEmpty().WithMessage("Due date is required")
            .Must(BeValidDate).WithMessage("Due date must be a valid date");
    }

    private bool BeValidDate(DateTime date)
    {
        return date != default(DateTime);
    }
}