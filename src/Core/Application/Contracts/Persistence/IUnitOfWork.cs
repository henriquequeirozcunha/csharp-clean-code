namespace Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        ILeaveAllocationRepository leaveAllocationRepository { get; }
        ILeaveRequestRepository leaveRequestRepository { get; }
        ILeaveTypeRepository leaveTypeRepository { get; }
        Task Save();
    }
}