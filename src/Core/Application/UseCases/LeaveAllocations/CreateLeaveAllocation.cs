using Application.Contracts.Identity;
using Application.Contracts.Persistence;
using Application.DTOs.LeaveAllocations;
using Application.Exceptions;
using Application.Responses;
using Application.UseCases.LeaveAllocations.Validators;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.UseCases.LeaveAllocations
{
    public class CreateLeaveAllocation
    {
        public class Command : IRequest<BaseCommandResponse>
        {
            public CreateLeaveAllocationDto CreateLeaveAllocationDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<CreateLeaveAllocationDto>
        {
            private readonly ILeaveTypeRepository _leaveTypeRepository;
            public CommandValidator(ILeaveTypeRepository leaveTypeRepository)
            {
                _leaveTypeRepository = leaveTypeRepository;

                RuleFor(p => p.LeaveTypeId)
                .GreaterThan(0)
                .MustAsync(async (id, token) =>
                {
                    return  await leaveTypeRepository.Exists(id);
                }).WithMessage("{PropertyName} does not exists");
            }
        }

        public class Handler : IRequestHandler<Command, BaseCommandResponse>
        {
            private readonly ILeaveAllocationRepository _leaveAllocationRepository;
            private readonly ILeaveTypeRepository _leaveTypeRepository;
            private readonly IMapper _mapper;
            private readonly IUserService _userService;
            public Handler(
                ILeaveAllocationRepository leaveAllocationRepository,
                ILeaveTypeRepository leaveTypeRepository,
                IMapper mapper,
                IUserService userService
                )
            {
                _userService = userService;
                _mapper = mapper;
                _leaveAllocationRepository = leaveAllocationRepository;
                _leaveTypeRepository = leaveTypeRepository;
            }

            public async Task<BaseCommandResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                var validator = new CommandValidator(_leaveTypeRepository);
                var validationResult = await validator.ValidateAsync(request.CreateLeaveAllocationDto);

                if (!validationResult.IsValid)
                {
                    return new BaseCommandResponse
                    {
                        Success = false,
                        Message = "Creation Failed",
                        Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList()
                    };
                    //throw new CustomValidationException(validationResult);
                };

                var leaveType = await _leaveTypeRepository.Get(request.CreateLeaveAllocationDto.LeaveTypeId);
                var employees = await _userService.GetEmployees();
                var period = DateTime.Now.Year;
                var allocations = new List<LeaveAllocation>();

                foreach (var employee in employees)
                {
                    if (await _leaveAllocationRepository.AllocationExists(employee.Id, leaveType.Id, period))
                    {
                        continue;
                    }

                    allocations.Add(new LeaveAllocation
                    {
                        EmployeeId = employee.Id,
                        LeaveTypeId = leaveType.Id,
                        NumberOfDays = leaveType.DefaultDays,
                        Period = period
                    });
                }

                await _leaveAllocationRepository.AddAllocations(allocations);

                return new BaseCommandResponse
                {
                    Success = true,
                    Message = "Allocation Successful"
                };
            }
        }
    }
}