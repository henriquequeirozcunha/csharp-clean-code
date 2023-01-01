using Application.Contracts.Persistence;
using Application.DTOs.LeaveAllocations;
using FluentValidation;

namespace Application.UseCases.LeaveAllocations.Validators
{
    public class ILeaveAllocationDtoValidator : AbstractValidator<ILeaveAllocationDto>
    {
        private readonly ILeaveAllocationRepository _repository;
        public ILeaveAllocationDtoValidator(ILeaveAllocationRepository repository)
        {
            _repository = repository;

            RuleFor(p => p.NumberOfDays)
                .GreaterThan(0).WithMessage("{PropertyName} must be before {ComparisonValue}");

            RuleFor(p => p.Period)
                .GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage("{PropertyName} must be after {ComparionValue}");

            RuleFor(p => p.LeaveTypeId)
                .GreaterThan(0)
                .MustAsync(async (id, token) =>
                {
                    return await _repository.Exists(id);
                });
        }
    }
}