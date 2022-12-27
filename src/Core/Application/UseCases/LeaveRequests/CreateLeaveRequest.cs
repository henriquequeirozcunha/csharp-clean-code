using Application.Contracts.Persistence;
using Application.DTOs.LeaveRequests;
using Application.Exceptions;
using Application.Responses;
using Application.UseCases.LeaveRequests.Validators;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.UseCases.LeaveRequests
{
    public class CreateLeaveRequest
    {
        public class Command : IRequest<BaseCommandResponse>
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

        public class Handler : IRequestHandler<Command, BaseCommandResponse>
        {
            private readonly ILeaveRequestRepository _repository;
            private readonly IMapper _mapper;
            public Handler(ILeaveRequestRepository repository, IMapper mapper)
            {
                _mapper = mapper;
                _repository = repository;

            }

            public async Task<BaseCommandResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                var response = new BaseCommandResponse();
                var validator = new CommandValidator(_repository);
                var validationResult = await validator.ValidateAsync(request.CreateLeaveRequestDto);

                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Creation Failed";
                    response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                     throw new CustomValidationException(validationResult);
                }

                var leaveRequest = _mapper.Map<LeaveRequest>(request.CreateLeaveRequestDto);

                leaveRequest = await _repository.Add(leaveRequest);

                response.Success = false;
                response.Message = "Creation Successfull";
                response.Id = leaveRequest.Id;

                return response;
            }
        }
    }
}