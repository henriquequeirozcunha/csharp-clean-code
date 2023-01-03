using Application.Contracts.Persistence;
using Application.DTOs.LeaveRequests;
using Application.Exceptions;
using Application.UseCases.LeaveRequests.Validators;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.UseCases.LeaveRequests
{
    public class UpdateLeaveRequest
    {
        public class Command : IRequest<Unit>
        {
            public int Id { get; set; }
            public UpdateLeaveRequestDto UpdateLeaveRequestDto { get; set; }
            public ChangeLeaveRequestApprovalDto ChangeLeaveRequestApprovalDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<UpdateLeaveRequestDto>
        {
            public CommandValidator(ILeaveRequestRepository repository)
            {
               Include(new ILeaveRequestDtoValidator(repository));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ILeaveRequestRepository _leaveRequestRepository;
            private readonly IMapper _mapper;
            private readonly ILeaveTypeRepository _leaveTypeRepository;
            private readonly ILeaveAllocationRepository _leaveAllocationRepository;
            public Handler(ILeaveRequestRepository leaveRequestRepository,
                IMapper mapper,
                ILeaveTypeRepository leaveTypeRepository,
                ILeaveAllocationRepository leaveAllocationRepository)
            {
                _mapper = mapper;
                _leaveRequestRepository = leaveRequestRepository;
                _leaveTypeRepository = leaveTypeRepository;
                _leaveAllocationRepository = leaveAllocationRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var leaveRequest = await _leaveRequestRepository.Get(request.Id);

                if (request.UpdateLeaveRequestDto != null)
                {
                    var validator = new CommandValidator(_leaveRequestRepository);
                    var validationResult = await validator.ValidateAsync(request.UpdateLeaveRequestDto);

                    if (!validationResult.IsValid) throw new CustomValidationException(validationResult);

                    _mapper.Map(request.UpdateLeaveRequestDto, leaveRequest);

                    await _leaveRequestRepository.Upadte(leaveRequest);
                } else if (request.ChangeLeaveRequestApprovalDto != null)
                {
                    await _leaveRequestRepository.ChangeApprovalStatus(leaveRequest, request.ChangeLeaveRequestApprovalDto.Approved);

                    if (request.ChangeLeaveRequestApprovalDto.Approved.Value)
                    {
                        var allocation = await _leaveAllocationRepository.GetUserAllocations(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId);

                        int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;

                        allocation.NumberOfDays -= daysRequested;

                        await _leaveAllocationRepository.Upadte(allocation);
                    }
                }

                return Unit.Value;
            }
        }
    }
}