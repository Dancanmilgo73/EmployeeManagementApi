using EmployeeManagementApi.Models.DTOs;

namespace EmployeeManagementApi.Application.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto?> GetByIdAsync(int id);
    Task<DepartmentDto> CreateAsync(DepartmentDto departmentDto);
    Task<DepartmentDto?> UpdateAsync(int id, DepartmentDto departmentDto);
    Task<bool> DeleteAsync(int id);
}
