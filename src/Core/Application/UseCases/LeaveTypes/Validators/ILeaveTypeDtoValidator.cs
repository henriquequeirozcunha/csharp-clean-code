using Application.Contracts.Persistence;
using Application.DTOs.LeaveTypes;
using FluentValidation;

namespace Application.UseCases.LeaveTypes.Validators
{
    public class ILeaveTypeDtoValidator : AbstractValidator<ILeaveTypeDto>
    {
        public ILeaveTypeDtoValidator()
        {
                RuleFor(p => p.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} is must not exceed {ComparisonValue} char");

                RuleFor(p => p.DefaultDays)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(1).WithMessage("{PropertyName} must be at least {ComparisonValue}")
                .LessThan(100).WithMessage("{PropertyName} must be at least {ComparisonValue}");
        }
    }
}