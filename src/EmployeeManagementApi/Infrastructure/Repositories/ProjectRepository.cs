
using EmployeeManagementApi.Infrastructure.Data;
using EmployeeManagementApi.Infrastructure.Repositories.Interfaces;
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

    public async Task<Project> GetByIdAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
            throw new InvalidOperationException($"Project with id {id} not found.");
        return project;
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _context.Projects.ToListAsync();
    }

    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
    }

    public async Task UpdateAsync(Project project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await GetByIdAsync(id);
        if (project != null)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }
        //what should we return after deletion?
        return false;
    }
    //Get projects by employee ID
    public async Task<IEnumerable<Project>> GetProjectsByEmployeeIdAsync(int employeeId)
    {
        return await _context.EmployeeProjects
            .Where(ep => ep.EmployeeId == employeeId && ep.Project != null)
            .Select(ep => ep.Project!)
            .ToListAsync();
    }
}