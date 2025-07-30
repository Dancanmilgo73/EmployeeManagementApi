using EmployeeManagementApi.Models.DTOs;

namespace EmployeeManagementApi.Application.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllAsync();
    Task<ProjectDto?> GetByIdAsync(int id);
    Task<ProjectDto> CreateAsync(ProjectDto projectDto);
    Task<ProjectDto?> UpdateAsync(int id, ProjectDto projectDto);
    Task<bool> DeleteAsync(int id);
}
