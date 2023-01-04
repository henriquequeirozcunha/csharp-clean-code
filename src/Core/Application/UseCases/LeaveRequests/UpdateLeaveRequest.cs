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
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            public Handler(IUnitOfWork unitOfWork,
                IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var leaveRequest = await _unitOfWork.leaveRequestRepository.Get(request.Id);

                if (leaveRequest is null) throw new NotFoundException(nameof(leaveRequest), request.Id);

                if (request.UpdateLeaveRequestDto != null)
                {
                    var validator = new CommandValidator(_unitOfWork.leaveRequestRepository);
                    var validationResult = await validator.ValidateAsync(request.UpdateLeaveRequestDto);

                    if (!validationResult.IsValid) throw new CustomValidationException(validationResult);

                    _mapper.Map(request.UpdateLeaveRequestDto, leaveRequest);

                    await _unitOfWork.leaveRequestRepository.Upadte(leaveRequest);
                    await _unitOfWork.Save();
                }
                else if (request.ChangeLeaveRequestApprovalDto != null)
                {
                    await _unitOfWork.leaveRequestRepository.ChangeApprovalStatus(leaveRequest, request.ChangeLeaveRequestApprovalDto.Approved);

                    if (request.ChangeLeaveRequestApprovalDto.Approved.Value)
                    {
                        var allocation = await _unitOfWork.leaveAllocationRepository.GetUserAllocations(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId);

                        int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;

                        allocation.NumberOfDays -= daysRequested;

                        await _unitOfWork.leaveAllocationRepository.Upadte(allocation);
                    }

                    await _unitOfWork.Save();
                }

                return Unit.Value;
            }
        }
    }
}