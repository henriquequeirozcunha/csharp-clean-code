using Application.Contracts.Identity;
using Application.Models.Identity;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<Employee>> GetEmployees()
        {
            var employess = await _userManager.GetUsersInRoleAsync("Employee");
            return employess.Select(q => new Employee {
                Id = q.Id,
                Email = q.Email,
                FirstName = q.FirstName,
                LastName = q.LastName
            }).ToList();
        }

        public async Task<Employee> GetEmployee(string userId)
        {
            var employee = await _userManager.FindByIdAsync(userId);

            return new Employee
            {
                Email = employee.Email,
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName
            };
        }
    }
}