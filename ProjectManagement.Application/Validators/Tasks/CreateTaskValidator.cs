using FluentValidation;
using ProjectManagement.Application.Commands.Tasks;

namespace ProjectManagement.Application.Validators.Tasks;

public class CreateTaskValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.ProjectId)
            .GreaterThan(0).WithMessage("Project ID must be greater than 0");

        RuleFor(x => x.Priority)
            .InclusiveBetween(1, 3).WithMessage("Priority must be between 1 and 3");

        RuleFor(x => x.DueDate)
            .NotEmpty().WithMessage("Due date is required")
            .GreaterThan(DateTime.Now).WithMessage("Due date must be in the future");
    }
}