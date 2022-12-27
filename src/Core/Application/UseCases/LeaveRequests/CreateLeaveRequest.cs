using Application.Contracts.Persistence;
using Application.DTOs.LeaveRequests;
using Application.UseCases.LeaveRequests.Validators;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.UseCases.LeaveRequests
{
    public class CreateLeaveRequest
    {
        public class Command : IRequest<int>
        {
            public CreateLeaveRequestDto CreateLeaveRequestDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<CreateLeaveRequestDto>
        {
            public CommandValidator(ILeaveRequestRepository repository)
            {
               Include(new ILeaveRequestDtoValidator(repository));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ILeaveRequestRepository _repository;
            private readonly IMapper _mapper;
            public Handler(ILeaveRequestRepository repository, IMapper mapper)
            {
                _mapper = mapper;
                _repository = repository;

            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var validator = new CommandValidator(_repository);
                var validationResult = await validator.ValidateAsync(request.CreateLeaveRequestDto);

                if (!validationResult.IsValid) throw new Exception();

                var leaveRequest = _mapper.Map<LeaveRequest>(request.CreateLeaveRequestDto);

                leaveRequest = await _repository.Add(leaveRequest);

                return leaveRequest.Id;
            }
        }
    }
}