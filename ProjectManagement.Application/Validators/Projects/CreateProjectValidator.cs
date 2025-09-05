using FluentValidation;
using ProjectManagement.Application.Commands.Projects;

namespace ProjectManagement.Application.Validators.Projects;

public class CreateProjectValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required")
            .MaximumLength(200).WithMessage("Project name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .Must(BeValidDate).WithMessage("Start date must be a valid date");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .Must(BeValidDate).WithMessage("End date must be a valid date")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");

        RuleFor(x => x.Budget)
            .GreaterThanOrEqualTo(0).WithMessage("Budget must be greater than or equal to 0");

        RuleFor(x => x.ManagerId)
            .GreaterThan(0).WithMessage("Manager ID must be greater than 0");
    }

    private bool BeValidDate(DateTime date)
    {
        return date != default(DateTime);
    }
}