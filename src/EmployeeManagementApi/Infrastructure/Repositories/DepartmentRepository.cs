using EmployeeManagementApi.Infrastructure.Data;
using EmployeeManagementApi.Infrastructure.Repositories.Interfaces;
using EmployeeManagementApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementApi.Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;
    public DepartmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Department?> GetByIdAsync(int id)
    {
        return await _context.Departments.FindAsync(id);
    }
    public async Task<IEnumerable<Department>> GetAllAsync() => await _context.Departments.ToListAsync();
    public async Task AddAsync(Department department)
    {
        await _context.Departments.AddAsync(department);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var department = await GetByIdAsync(id);
        if (department == null) return false;
        _context.Departments.Remove(department);
        return true;
    }
    public async Task<IEnumerable<Project>> GetProjectsByDepartmentIdAsync(int departmentId)
    {
        return await _context.Departments
            .Where(d => d.Id == departmentId)
            .SelectMany(d => d.Projects)
            .AsNoTracking()
            .ToListAsync();
    }
}
