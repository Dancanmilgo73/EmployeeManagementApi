using EmployeeManagementApi.Application.Interfaces;
using EmployeeManagementApi.Infrastructure.Repositories.Interfaces;
using EmployeeManagementApi.Models.DTOs;
using EmployeeManagementApi.Models.Entities;
namespace EmployeeManagementApi.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IRandomStringGeneratorService _randomStringGeneratorService;

    public EmployeeService(IEmployeeRepository employeeRepository, IRandomStringGeneratorService randomStringGeneratorService)
    {
        _employeeRepository = employeeRepository;
        _randomStringGeneratorService = randomStringGeneratorService;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return employees.Select(e => new EmployeeDto
        {
            Id = e.Id,
            // Name = e.Name,
            // Position = e.Position,
            Salary = e.Salary
        });
    }

    public async Task<EmployeeDto> GetByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null) return null;
        return new EmployeeDto
        {
            Id = employee.Id,
            // Name = employee.Name,
            // Position = employee.Position,
            Salary = employee.Salary
        };
    }

    public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        return new EmployeeDto
        {
            Id = employee.Id,
            // Name = employee.Name,
            // Position = employee.Position,
            Salary = employee.Salary
        };
    }

    public async Task<EmployeeDto> CreateAsync(EmployeeDto employeeDto)
    {
        var employee = new Employee
        {
            // Name = employeeDto.Name,
            // Position = employeeDto.Position,
            Salary = employeeDto.Salary
        };
        var createdEmployee = await _employeeRepository.AddAsync(employee);
        return new EmployeeDto
        {
            Id = createdEmployee.Id,
            // Name = createdEmployee.Name,
            // Position = createdEmployee.Position,
            Salary = createdEmployee.Salary
        };
    }

    public async Task<EmployeeDto> UpdateAsync(int id, EmployeeDto employeeDto)
    {
        var employee = new Employee
        {
            Id = id,
            // Name = employeeDto.Name,
            // Position = employeeDto.Position,
            Salary = employeeDto.Salary
        };
        var updatedEmployee = await _employeeRepository.UpdateAsync(employee);
        return new EmployeeDto
        {
            Id = updatedEmployee.Id,
            // Name = updatedEmployee.Name,
            // Position = updatedEmployee.Position,
            Salary = updatedEmployee.Salary
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _employeeRepository.DeleteAsync(id);
    }

    // public async Task<string> GenerateRandomEmployeeCodeAsync(int length)
    // {
    //     return await _randomStringGeneratorService.GenerateRandomStringAsync(length);
    // }
}