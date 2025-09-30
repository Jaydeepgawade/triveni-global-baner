using EmployeeManagement.Application.DTOs;
using FluentValidation;

namespace EmployeeManagement.Application.Validators;

public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
{
    public CreateEmployeeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Employee name is required")
            .MaximumLength(100).WithMessage("Employee name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required")
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters");

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required")
            .Must(g => g.ToLower() == "male" || g.ToLower() == "female" || g.ToLower() == "other")
            .WithMessage("Gender must be Male, Female, or Other");

        RuleFor(x => x.DOB)
            .NotEmpty().WithMessage("Date of birth is required")
            .LessThan(DateTime.Now.AddYears(-18)).WithMessage("Employee must be at least 18 years old")
            .GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Invalid date of birth");

        RuleFor(x => x.DeptId)
            .GreaterThan(0).WithMessage("Department ID must be greater than 0");
    }
}

public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
{
    public UpdateEmployeeDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Employee ID must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Employee name is required")
            .MaximumLength(100).WithMessage("Employee name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required")
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters");

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required")
            .Must(g => g.ToLower() == "male" || g.ToLower() == "female" || g.ToLower() == "other")
            .WithMessage("Gender must be Male, Female, or Other");

        RuleFor(x => x.DOB)
            .NotEmpty().WithMessage("Date of birth is required")
            .LessThan(DateTime.Now.AddYears(-18)).WithMessage("Employee must be at least 18 years old")
            .GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Invalid date of birth");

        RuleFor(x => x.DeptId)
            .GreaterThan(0).WithMessage("Department ID must be greater than 0");
    }
}
