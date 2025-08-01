using EmployeeManagementApi.Application.Interfaces;
using EmployeeManagementApi.Infrastructure.Repositories.Interfaces;
using EmployeeManagementApi.Models.DTOs;
using EmployeeManagementApi.Models.Entities;
namespace EmployeeManagementApi.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRandomStringGeneratorService _randomStringGeneratorService;

    public EmployeeService(IUnitOfWork unitOfWork, IRandomStringGeneratorService randomStringGeneratorService)
    {
        _unitOfWork = unitOfWork;
        _randomStringGeneratorService = randomStringGeneratorService;
    }
    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        try
        {
            var employees = await _unitOfWork.Employees.GetAllAsync();
            return employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                DepartmentId = e.DepartmentId,
                Salary = e.Salary
            });
        }
        catch (System.Exception)
        {
            throw new ApplicationException("An error occurred while retrieving employees.");
        }
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        try
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);
            if (employee == null) return null;
            return new EmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                DepartmentId = employee.DepartmentId,
                Salary = employee.Salary
            };
        }
        catch (System.Exception)
        {
            throw new ApplicationException("An error occurred while retrieving the employee.");
        }
    }

    public async Task<int> CreateAsync(EmployeeCreateDto employeeDto)
    {
        try
        {
            var depId = employeeDto.DepartmentId == 0 || employeeDto.DepartmentId == null
                ? 1 // Default to HR department if not specified
                : employeeDto.DepartmentId.Value;
            var employee = new Employee
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Email = employeeDto.Email,
                Salary = employeeDto.Salary,
                DepartmentId = depId
            };
            await _unitOfWork.Employees.AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();
            return employee.Id;
        }
        catch (System.Exception)
        {
            throw new ApplicationException("An error occurred while creating the employee. Be sure to enter valid data.");
        }
    }

    public async Task<int?> UpdateAsync(int id, EmployeeUpdateDto employeeDto)
    {
        if (employeeDto == null || id <= 0) return null;

        try
        {
            var existingEmployee = await _unitOfWork.Employees.GetByIdAsync(id);
            if (existingEmployee == null) return null;
            existingEmployee.FirstName = employeeDto.FirstName;
            existingEmployee.LastName = employeeDto.LastName;
            existingEmployee.Email = employeeDto.Email;
            existingEmployee.DepartmentId = employeeDto.DepartmentId;
            existingEmployee.Salary = employeeDto.Salary;
            await _unitOfWork.SaveChangesAsync();
            return existingEmployee.Id;
        }
        catch (System.Exception)
        {
            throw new ApplicationException("An error occurred while updating the employee. Be sure to enter valid data.");
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var deleted = await _unitOfWork.Employees.DeleteAsync(id);
            if (!deleted) return false;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (System.Exception)
        {
            throw new ApplicationException("An error occurred while deleting the employee.");
        }

    }
    public async Task<IEnumerable<ProjectDto>> GetProjectsByEmployeeIdAsync(int employeeId)
    {
        try
        {
            var projects = await _unitOfWork.Projects.GetProjectsByEmployeeIdAsync(employeeId);
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
            throw new ApplicationException("An error occurred while retrieving projects for the employee.");
        }
    }
}