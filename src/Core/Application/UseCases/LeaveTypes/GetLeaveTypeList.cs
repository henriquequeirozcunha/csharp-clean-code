using Application.Contracts.Persistence;
using Application.DTOs.LeaveTypes;
using AutoMapper;
using MediatR;

namespace Application.UseCases.LeaveTypes
{
    public class GetLeaveTypeListRequest
    {
        public class Query : IRequest<List<LeaveTypeDto>> {}

        public class Handler : IRequestHandler<Query, List<LeaveTypeDto>>
        {
            private readonly ILeaveTypeRepository _leaveTypeRepository;
            private readonly IMapper _mapper;

            public Handler(ILeaveTypeRepository leaveTypeRepository, IMapper mapper)
            {
                _leaveTypeRepository = leaveTypeRepository;
                _mapper = mapper;
            }

            public async Task<List<LeaveTypeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var leaveTypes = await _leaveTypeRepository.GetAll();
                return _mapper.Map<List<LeaveTypeDto>>(leaveTypes);
            }
        }
    }
}
