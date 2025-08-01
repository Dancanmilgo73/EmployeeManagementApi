using EmployeeManagementApi.Models.DTOs;

namespace EmployeeManagementApi.Application.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllAsync();
    Task<ProjectDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(ProjectCreateDto projectDto);
    Task<int?> UpdateAsync(int id, ProjectUpdateDto projectDto);
    Task<bool> DeleteAsync(int id);
    Task<bool> AssignEmployeeToProjectAsync(EmployeeProjectDto employeeProjectDto);
    Task<bool> RemoveEmployeeFromProjectAsync(EmployeeProjectDto employeeProjectDto);
    Task<decimal> GetTotalBudgetAsync(int departmentId);
}
