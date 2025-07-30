using EmployeeManagementApi.Models.Entities;

namespace EmployeeManagementApi.Infrastructure.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee> GetByIdAsync(int id);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee> AddAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Employee>> GetEmployeesByProjectIdAsync(int projectId);
}