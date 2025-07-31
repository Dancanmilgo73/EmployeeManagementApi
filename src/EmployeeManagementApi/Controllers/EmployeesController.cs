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
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await _employeeService.GetAllAsync());
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var employee = await _employeeService.GetByIdAsync(id);
            return employee == null ? NotFound() : Ok(employee);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmployeeCreateDto employeeDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var id = await _employeeService.CreateAsync(employeeDto);
            return CreatedAtAction(nameof(GetById), new { id }, employeeDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] EmployeeDto employeeDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var updatedId = await _employeeService.UpdateAsync(id, employeeDto);
            return updatedId == null ? NotFound() : CreatedAtAction(nameof(GetById), new { id = updatedId }, employeeDto);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _employeeService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }
    [HttpGet("{id}/projects")]
    public async Task<IActionResult> GetProjectsByEmployeeId(int id)
    {
        try
        {
            var projects = await _employeeService.GetProjectsByEmployeeIdAsync(id);
            return Ok(projects);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }
}