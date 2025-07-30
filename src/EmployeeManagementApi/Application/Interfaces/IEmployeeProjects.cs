using EmployeeManagementApi.Models.DTOs;

namespace EmployeeManagementApi.Application.Interfaces;
public interface IEmployeeProjects
{
    Task<IEnumerable<ProjectDto>> GetProjectsByEmployeeIdAsync(int employeeId);
    Task<ProjectDto> AddProjectToEmployeeAsync(int employeeId, ProjectDto projectDto);
    Task<bool> RemoveProjectFromEmployeeAsync(int employeeId, int projectId);
    Task<bool> UpdateProjectForEmployeeAsync(int employeeId, int projectId, ProjectDto projectDto);
    
}