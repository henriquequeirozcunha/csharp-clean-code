using Application.Contracts.Persistence;
using Domain.Entities;

namespace tests.Mocks
{
    public class MockUnitOfWork
    {
        public static Mock<IUnitOfWork> GetUnitOfWork()
        {
            var mockUoW = new Mock<IUnitOfWork>();

            var mockLeaveTypeRepo = MockLeaveTypeRepository.GetLeaveTypeRepository();

            mockUoW.Setup(r => r.leaveTypeRepository).Returns(mockLeaveTypeRepo.Object);

            return mockUoW;
        }
    }
}