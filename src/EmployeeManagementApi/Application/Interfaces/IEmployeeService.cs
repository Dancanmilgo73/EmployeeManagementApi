using EmployeeManagementApi.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace EmployeeManagementApi.Application.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync();
    Task<EmployeeDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(EmployeeCreateDto employeeDto);
    Task<int?> UpdateAsync(int id, EmployeeDto employeeDto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ProjectDto>> GetProjectsByEmployeeIdAsync(int employeeId);
}
