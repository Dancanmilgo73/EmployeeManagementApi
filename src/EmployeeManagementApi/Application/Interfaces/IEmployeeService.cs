using EmployeeManagementApi.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace EmployeeManagementApi.Application.Interfaces;
public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync();
    Task<EmployeeDto> GetByIdAsync(int id);
    Task<EmployeeDto> CreateAsync(EmployeeDto employeeDto);
    Task<EmployeeDto> UpdateAsync(int id, EmployeeDto employeeDto);
    Task<bool> DeleteAsync(int id);
}
