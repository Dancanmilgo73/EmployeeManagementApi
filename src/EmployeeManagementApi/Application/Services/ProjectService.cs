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
    public async Task<ProjectDto> CreateAsync(ProjectDto projectDto)
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
            await _unitOfWork.Projects.UpdateAsync(project);
            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();
            return new ProjectDto { Id = project.Id, Name = project.Name, Budget = project.Budget, ProjectCode = project.ProjectCode };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync()
    {
        var projects = await _unitOfWork.Projects.GetAllAsync();
        return projects.Select(p => new ProjectDto
        {
            Id = p.Id,
            Name = p.Name,
            // Description = p.Description,
            // StartDate = p.StartDate,
            // EndDate = p.EndDate
        });
    }

    public async Task<ProjectDto?> GetByIdAsync(int id)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(id);
        if (project == null) return null;

        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            // Description = project.Description,
            // StartDate = project.StartDate,
            // EndDate = project.EndDate
        };
    }

    public async Task<ProjectDto?> UpdateAsync(int id, ProjectDto projectDto)
    {
        var existingProject = await _unitOfWork.Projects.GetByIdAsync(id);
        if (existingProject == null) return null;

        existingProject.Name = projectDto.Name;
                // existingProject.Description = projectDto.Description;
                // existingProject.StartDate = projectDto.StartDate;
                // existingProject.EndDate = projectDto.EndDate;

        await _unitOfWork.Projects.UpdateAsync(existingProject);
        await _unitOfWork.SaveChangesAsync();
        return new ProjectDto
        {
            Id = existingProject.Id,
            Name = existingProject.Name,
            // Description = existingProject.Description,
            // StartDate = existingProject.StartDate,
            // EndDate = existingProject.EndDate
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _unitOfWork.Projects.DeleteAsync(id);
    }
}
