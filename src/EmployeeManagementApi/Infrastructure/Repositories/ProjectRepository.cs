
using EmployeeManagementApi.Infrastructure.Data;
using EmployeeManagementApi.Infrastructure.Repositories.Interfaces;
using EmployeeManagementApi.Models.DTOs;
using EmployeeManagementApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementApi.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects.FindAsync(id);
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _context.Projects.AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await GetByIdAsync(id);
        if (project == null) return false;
        _context.Projects.Remove(project);
        return true;
    }
    //Get projects by employee ID
    public async Task<IEnumerable<Project>> GetProjectsByEmployeeIdAsync(int employeeId)
    {
        return await _context.EmployeeProjects
            .Where(ep => ep.EmployeeId == employeeId && ep.Project != null)
            .Select(ep => ep.Project!)
            .ToListAsync();
    }
    //Assign employee to project
    public async Task<bool> AssignEmployeeToProjectAsync(EmployeeProjectDto employeeProject)
    {
        await _context.EmployeeProjects.AddAsync(new EmployeeProject
        {
            EmployeeId = employeeProject.EmployeeId,
            ProjectId = employeeProject.ProjectId,
            Role = employeeProject.Role
        });
        return true;
    }

    //Remove employee from project
    public bool RemoveEmployeeFromProjectAsync(EmployeeProjectDto employeeProject)
    {
        var entity = _context.EmployeeProjects.Find(employeeProject.EmployeeId, employeeProject.ProjectId);
        if (entity == null) return false;

        _context.EmployeeProjects.Remove(entity);
        return true;
    }
}