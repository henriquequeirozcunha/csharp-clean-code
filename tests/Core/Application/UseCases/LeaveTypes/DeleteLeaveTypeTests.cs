using Application.Contracts.Persistence;
using Application.DTOs.LeaveTypes;
using Application.Exceptions;
using Application.UseCases.LeaveTypes;
using AutoMapper;
using Domain.Entities;
using tests.Mocks;

namespace tests.Core.Application.UseCases.LeaveTypes
{
    [TestFixture]
    public class DeleteLeaveTypeTests
    {
        private IMapper _mapper;
        private Mock<IUnitOfWork> _mockUow;
        private DeleteLeaveType.Handler _sut;
        private Mock<ILeaveTypeRepository> _mockLeaveTypeRepo;
        private LeaveType _getLeaveTypeResult;

        [SetUp]
        public void MakeSut()
        {
            _getLeaveTypeResult = new LeaveType { Id = 1, Name = "Any Name" };

            _mockLeaveTypeRepo = new Mock<ILeaveTypeRepository>();
            _mockLeaveTypeRepo.Setup(r => r.Get(It.IsAny<int>())).ReturnsAsync(() => _getLeaveTypeResult);

            _mockUow = MockUnitOfWork.GetUnitOfWork();
            _mockUow.Setup(r => r.leaveTypeRepository).Returns(_mockLeaveTypeRepo.Object);

            var mapperConfig = new MapperConfiguration(c => {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            _sut = new DeleteLeaveType.Handler(_mockUow.Object, _mapper);
        }

        [Test(Description = "Should call leaveTypeRepository Get with correct params")]
        public async Task should_call_leave_type_repository_get_with_correct_params()
        {
            var request = new DeleteLeaveType.Command() { Id = 1 };

            await _sut.Handle(request, CancellationToken.None);

            _mockUow.Verify(r => r.leaveTypeRepository.Get(request.Id), Times.Once);
        }

        [Test(Description = "Should Throw NotFoundException if LeaveType is not found")]
        public async Task should_throw_not_found_exception_on_invalid_id()
        {
            _getLeaveTypeResult = null;

            var promise = _sut.Handle(new DeleteLeaveType.Command() { Id = 1 }, CancellationToken.None);

            var ex = await Should.ThrowAsync<NotFoundException>(promise);

            ex.Message.ShouldBe("LeaveType (1) was not found");

            Assert.That(ex.Message, Is.EqualTo("LeaveType (1) was not found"));
            Assert.That(ex, Is.TypeOf<NotFoundException>());
        }

        [Test(Description = "Should Throws an Exception on Error")]
        public async Task should_throw_an_exception_on_error()
        {
            _mockLeaveTypeRepo.Setup(r => r.Remove(It.IsAny<LeaveType>())).ThrowsAsync(new Exception("LeaveRepositoryError"));

            var promise = _sut.Handle(new DeleteLeaveType.Command() { Id = 1 }, CancellationToken.None);

            var ex = await Should.ThrowAsync<Exception>(promise);

            ex.Message.ShouldBe("LeaveRepositoryError");
            ex.ShouldBeOfType<Exception>();

            Assert.That(ex.Message, Is.EqualTo("LeaveRepositoryError"));
            Assert.That(ex, Is.TypeOf<Exception>());
        }

        [Test(Description = "Should call leaveTypeRepository Remove with correct params and call unitOfWork Save() on Success")]
        public async Task should_call_leave_type_repository_remove_with_correct_params()
        {
            var request = new DeleteLeaveType.Command() { Id = 1 };

            var result = await _sut.Handle(request, CancellationToken.None);

            _mockUow.Verify(r => r.leaveTypeRepository.Remove(_getLeaveTypeResult), Times.Once);

            _mockUow.Verify(r => r.Save(), Times.Once);
        }
    }
}