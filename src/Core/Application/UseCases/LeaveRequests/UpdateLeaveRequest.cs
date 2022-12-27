using Application.Contracts.Persistence;
using Application.DTOs.LeaveRequests;
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
            public UpdateLeaveRequestDto UpdateLeaveRequestDto { get; set; }
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
            private readonly ILeaveRequestRepository _repository;
            private readonly IMapper _mapper;
            public Handler(ILeaveRequestRepository repository, IMapper mapper)
            {
                _mapper = mapper;
                _repository = repository;

            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var validator = new CommandValidator(_repository);
                var validationResult = await validator.ValidateAsync(request.UpdateLeaveRequestDto);

                if (!validationResult.IsValid) throw new Exception();

                var leaveRequest = await _repository.Get(request.UpdateLeaveRequestDto.Id);

                _mapper.Map(request.UpdateLeaveRequestDto, leaveRequest);

                await _repository.Upadte(leaveRequest);

                return Unit.Value;
            }
        }
    }
}