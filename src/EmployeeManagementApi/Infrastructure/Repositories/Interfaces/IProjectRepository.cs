using EmployeeManagementApi.Models.Entities;

namespace EmployeeManagementApi.Infrastructure.Repositories.Interfaces;

public interface IProjectRepository
{
    Task<Project> GetByIdAsync(int id);
    Task<IEnumerable<Project>> GetAllAsync();
    Task AddAsync(Project project);
    Task UpdateAsync(Project project);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Project>> GetProjectsByEmployeeIdAsync(int employeeId);
}