using Application.Models.Identity;

namespace Application.Contracts.Identity
{
    public interface IUserService
    {
         Task<List<Employee>> GetEmployees();
    }
}