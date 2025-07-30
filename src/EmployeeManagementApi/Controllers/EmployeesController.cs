using EmployeeManagementApi.Application.Interfaces;
using EmployeeManagementApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeManagementApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _employeeService.GetAllAsync());
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        return employee == null ? NotFound() : Ok(employee);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmployeeDto employeeDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var employee = await _employeeService.CreateAsync(employeeDto);
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] EmployeeDto employeeDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updatedEmployee = await _employeeService.UpdateAsync(id, employeeDto);
        return updatedEmployee == null ? NotFound() : Ok(updatedEmployee);
    }   
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _employeeService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}