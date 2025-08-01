using EmployeeManagementApi.Models.DTOs;

namespace EmployeeManagementApi.Application.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(DepartmentCreateDto departmentDto);
    Task<int?> UpdateAsync(int id, DepartmentUpdateDto departmentDto);
    Task<bool> DeleteAsync(int id);
}
