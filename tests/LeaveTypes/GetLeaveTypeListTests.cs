using Application.Contracts.Persistence;
using Application.DTOs.LeaveTypes;
using Application.UseCases.LeaveTypes;
using AutoMapper;
using Shouldly;
using tests.Mocks;

namespace tests.LeaveTypes
{
    public class GetLeaveTypeListTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveTypeRepository> _mockRepo;
        public GetLeaveTypeListTests()
        {
            _mockRepo = MockLeaveTypeRepository.GetLeaveTypeRepository();

            var mapperConfig = new MapperConfiguration(c => {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task GetLeaveTypeList()
        {
            var sut = new GetLeaveTypeList.Handler(_mockRepo.Object, _mapper);

            var result = await sut.Handle(new GetLeaveTypeList.Query(), CancellationToken.None);

            result.ShouldBeOfType<List<LeaveTypeDto>>();

            result.Count.ShouldBe(2);
        }
    }
}