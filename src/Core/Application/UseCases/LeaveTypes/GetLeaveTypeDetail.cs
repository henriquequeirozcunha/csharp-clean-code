using Application.Contracts.Persistence;
using Application.DTOs.LeaveTypes;
using AutoMapper;
using MediatR;

namespace Application.UseCases.LeaveTypes
{
    public class GetLeaveTypeDetail
    {
        public class Query : IRequest<LeaveTypeDto>
        {
            public int Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, LeaveTypeDto>
        {
            private readonly ILeaveTypeRepository _leaveTypeRepository;
            private readonly IMapper _mapper;

            public Handler(ILeaveTypeRepository leaveTypeRepository, IMapper mapper)
            {
                _leaveTypeRepository = leaveTypeRepository;
                _mapper = mapper;
            }
            public async Task<LeaveTypeDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var leaveType = await _leaveTypeRepository.Get(request.Id);
                return _mapper.Map<LeaveTypeDto>(leaveType);
            }
        }
    }
}