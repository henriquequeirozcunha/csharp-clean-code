using Application.Contracts.Persistence;
using Application.DTOs.LeaveRequests;
using FluentValidation;

namespace Application.UseCases.LeaveRequests.Validators
{
    public class ILeaveRequestDtoValidator : AbstractValidator<ILeaveRequestDto>
    {
        private readonly ILeaveRequestRepository _repository;
        public ILeaveRequestDtoValidator(ILeaveRequestRepository repository)
        {
             _repository = repository;

            RuleFor(p => p.StartDate).LessThan(p => p.EndDate).WithMessage("{PropertyName} must be before {ComparisonValue}");
            RuleFor(p => p.EndDate).LessThan(p => p.StartDate).WithMessage("{PropertyName} must be after {ComparisonValue}");

            RuleFor(p => p.LeaveTypeId)
            .GreaterThan(0)
            .MustAsync(async (id, token) => {
            var leaveTypeExists = await _repository.Exists(id);

            return !leaveTypeExists;
            });
        }
    }
}