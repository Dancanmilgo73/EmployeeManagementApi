using EmployeeManagementApi.Application.Interfaces;
using EmployeeManagementApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _projectService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var project = await _projectService.GetByIdAsync(id);
        return project == null ? NotFound() : Ok(project);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProjectDto projectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var project = await _projectService.CreateAsync(projectDto);
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProjectDto projectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updatedProject = await _projectService.UpdateAsync(id, projectDto);
        return updatedProject == null ? NotFound() : Ok(updatedProject);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _projectService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
    // [HttpGet("employees/{projectId}")]
    // public async Task<IActionResult> GetEmployeesByProjectId(int projectId)
    // {
    //     var employees = await _projectService.GetEmployeesByProjectIdAsync(projectId);
    //     return employees == null ? NotFound() : Ok(employees);
    // }
    // [HttpPost("assign")]
    // public async Task<IActionResult> AssignEmployeeToProject([FromBody] EmployeeProjectDto employeeProjectDto)
    // {
    //     if (!ModelState.IsValid) return BadRequest(ModelState);
    //     var result = await _projectService.AssignEmployeeToProjectAsync(employeeProjectDto);
    //     return result ? Ok() : BadRequest("Failed to assign employee to project.");
    // }
}