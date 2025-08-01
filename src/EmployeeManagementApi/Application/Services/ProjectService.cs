using EmployeeManagementApi.Application.Interfaces;
using EmployeeManagementApi.Infrastructure.Repositories.Interfaces;
using EmployeeManagementApi.Models.DTOs;
using EmployeeManagementApi.Models.Entities;

namespace EmployeeManagementApi.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRandomStringGeneratorService _randomStringGenerator;
    public ProjectService(IUnitOfWork unitOfWork, IRandomStringGeneratorService randomStringGenerator)
    {
        _unitOfWork = unitOfWork;
        _randomStringGenerator = randomStringGenerator;
    }
    public async Task<int> CreateAsync(ProjectCreateDto projectDto)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var project = new Project
            {
                Name = projectDto.Name,
                Budget = projectDto.Budget
            };
            await _unitOfWork.Projects.AddAsync(project);
            await _unitOfWork.SaveChangesAsync();
            var randomCode = await _randomStringGenerator.GenerateRandomStringAsync(10);
            project.ProjectCode = $"{randomCode}-{project.Id}";
            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();
            return project.Id;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw new ApplicationException("An error occurred while creating the project.");
        }
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync()
    {
        try
        {
            var projects = await _unitOfWork.Projects.GetAllAsync();
            return projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                ProjectCode = p.ProjectCode ?? string.Empty,
                Budget = p.Budget
            });
        }
        catch (System.Exception)
        {
            throw new ApplicationException("An error occurred while retrieving projects.");
        }
    }

    public async Task<ProjectDto?> GetByIdAsync(int id)
    {
        try
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);
            if (project == null) return null;

            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                ProjectCode = project.ProjectCode ?? string.Empty,
                Budget = project.Budget
            };
        }
        catch (System.Exception)
        {
            throw new ApplicationException("An error occurred while retrieving the project.");
        }
    }

    public async Task<int?> UpdateAsync(int id, ProjectUpdateDto projectDto)
    {
        try
        {
            var existingProject = await _unitOfWork.Projects.GetByIdAsync(id);
            if (existingProject == null) return null;

            existingProject.Name = projectDto.Name;
            existingProject.Budget = projectDto.Budget;
            await _unitOfWork.SaveChangesAsync();
            return existingProject.Id == 0 ? null : existingProject.Id;
        }
        catch (System.Exception)
        {
            throw new ApplicationException("An error occurred while updating the project.");
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var deleted = await _unitOfWork.Projects.DeleteAsync(id);
            if (!deleted) return false;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (System.Exception)
        {
            throw new ApplicationException("An error occurred while deleting the project.");
        }
    }
    public async Task<bool> AssignEmployeeToProjectAsync(EmployeeProjectDto employeeProjectDto)
    {
        try
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(employeeProjectDto.ProjectId);
            if (project == null) throw new ApplicationException("Project not found.");

            var employee = await _unitOfWork.Employees.GetByIdAsync(employeeProjectDto.EmployeeId);
            if (employee == null) throw new ApplicationException("Employee not found.");

            var assigned = await _unitOfWork.Projects.AssignEmployeeToProjectAsync(employeeProjectDto);
            await _unitOfWork.SaveChangesAsync();
            return assigned;
        }
        catch (System.Exception)
        {
            throw new ApplicationException("An error occurred while assigning the employee to the project.");
        }
    }

    public async Task<bool> RemoveEmployeeFromProjectAsync(EmployeeProjectDto employeeProjectDto)
    {
        try
        {
            var deleted = _unitOfWork.Projects.RemoveEmployeeFromProjectAsync(employeeProjectDto);
            if (!deleted) return false;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (System.Exception)
        {
            throw new ApplicationException("An error occurred while removing the employee from the project.");
        }
    }
    public async Task<decimal> GetTotalBudgetAsync(int departmentId)
    {
        try
        {
            var projects = await _unitOfWork.Departments.GetProjectsByDepartmentIdAsync(departmentId);
            return projects.Sum(p => p.Budget);
        }
        catch (System.Exception)
        {
            throw new ApplicationException("An error occurred while calculating the total budget.");
        }
    }
}