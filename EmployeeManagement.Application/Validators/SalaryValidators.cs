using EmployeeManagement.Application.DTOs;
using FluentValidation;

namespace EmployeeManagement.Application.Validators;

public class CreateSalaryDtoValidator : AbstractValidator<CreateSalaryDto>
{
    public CreateSalaryDtoValidator()
    {
        RuleFor(x => x.EmpId)
            .GreaterThan(0).WithMessage("Employee ID must be greater than 0");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Salary amount must be greater than 0")
            .LessThan(1000000).WithMessage("Salary amount cannot exceed 1,000,000");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Salary date is required")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Salary date cannot be in the future");
    }
}

public class UpdateSalaryDtoValidator : AbstractValidator<UpdateSalaryDto>
{
    public UpdateSalaryDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Salary ID must be greater than 0");

        RuleFor(x => x.EmpId)
            .GreaterThan(0).WithMessage("Employee ID must be greater than 0");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Salary amount must be greater than 0")
            .LessThan(1000000).WithMessage("Salary amount cannot exceed 1,000,000");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Salary date is required")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Salary date cannot be in the future");
    }
}
