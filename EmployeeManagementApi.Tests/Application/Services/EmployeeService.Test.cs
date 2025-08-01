using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementApi.Application.Interfaces;
using EmployeeManagementApi.Application.Services;
using EmployeeManagementApi.Infrastructure.Repositories.Interfaces;
using EmployeeManagementApi.Models.DTOs;
using EmployeeManagementApi.Models.Entities;
using Moq;
using Xunit;

namespace EmployeeManagementApi.Application.Services.Tests
{
    public class EmployeeServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEmployeeRepository> _employeeRepoMock;
        private readonly Mock<IProjectRepository> _projectRepoMock;
        private readonly Mock<IRandomStringGeneratorService> _randomStringGeneratorMock;
        private readonly EmployeeService _service;

        public EmployeeServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _employeeRepoMock = new Mock<IEmployeeRepository>();
            _projectRepoMock = new Mock<IProjectRepository>();
            _randomStringGeneratorMock = new Mock<IRandomStringGeneratorService>();

            _unitOfWorkMock.SetupGet(u => u.Employees).Returns(_employeeRepoMock.Object);
            _unitOfWorkMock.SetupGet(u => u.Projects).Returns(_projectRepoMock.Object);

            _service = new EmployeeService(_unitOfWorkMock.Object, _randomStringGeneratorMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmployeeDtos()
        {
            var employees = new List<Employee>
            {
                new Employee { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@company.com", DepartmentId = 2, Salary = 50000 },
                new Employee { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@company.com", DepartmentId = 3, Salary = 60000 }
            };
            _employeeRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(employees);

            var result = await _service.GetAllAsync();

            Assert.Equal(2, result.Count());
            Assert.Contains(result, e => e.FirstName == "John" && e.LastName == "Doe" && e.Email == "john@company.com");
            Assert.Contains(result, e => e.FirstName == "Jane" && e.LastName == "Smith" && e.Email == "jane@company.com");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsEmployeeDto_WhenFound()
        {
            var employee = new Employee { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@company.com", DepartmentId = 2, Salary = 50000 };
            _employeeRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employee);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
            Assert.Equal("john@company.com", result.Email);
            Assert.Equal(2, result.DepartmentId);
            Assert.Equal(50000, result.Salary);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            _employeeRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Employee?)null);

            var result = await _service.GetByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_AddsEmployeeAndReturnsId()
        {
            var dto = new EmployeeCreateDto { FirstName = "Alice", LastName = "Wonder", Email = "alice@company.com", Salary = 70000, DepartmentId = 5 };
            var employee = new Employee { Id = 10, FirstName = "Alice", LastName = "Wonder", Email = "alice@company.com", Salary = 70000, DepartmentId = 5 };

            _employeeRepoMock.Setup(r => r.AddAsync(It.IsAny<Employee>()))
                .Callback<Employee>(e => e.Id = employee.Id)
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.CreateAsync(dto);

            Assert.Equal(10, result);
        }

        [Fact]
        public async Task CreateAsync_DefaultsDepartmentIdTo1_WhenNotSpecified()
        {
            var dto = new EmployeeCreateDto { FirstName = "Bob", LastName = "Builder", Email = "bob@company.com", Salary = 40000, DepartmentId = null };
            var employee = new Employee { Id = 11, FirstName = "Bob", LastName = "Builder", Email = "bob@company.com", Salary = 40000, DepartmentId = 1 };

            _employeeRepoMock.Setup(r => r.AddAsync(It.IsAny<Employee>()))
                .Callback<Employee>(e => e.Id = employee.Id)
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.CreateAsync(dto);

            Assert.Equal(11, result);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenEmployeeNotFound()
        {
            _employeeRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Employee?)null);

            var dto = new EmployeeUpdateDto { FirstName = "Any", LastName = "Any", Email = "any@company.com", DepartmentId = 1, Salary = 10000 };
            var result = await _service.UpdateAsync(99, dto);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenDtoIsNullOrIdInvalid()
        {
            var result1 = await _service.UpdateAsync(0, null);
            var result2 = await _service.UpdateAsync(-1, new EmployeeUpdateDto());

            Assert.Null(result1);
            Assert.Null(result2);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesEmployeeAndReturnsId()
        {
            var employee = new Employee { Id = 5, FirstName = "Old", LastName = "Name", Email = "old@company.com", DepartmentId = 2, Salary = 30000 };
            _employeeRepoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(employee);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var dto = new EmployeeUpdateDto { FirstName = "New", LastName = "Name", Email = "new@company.com", DepartmentId = 3, Salary = 35000 };
            var result = await _service.UpdateAsync(5, dto);

            Assert.Equal(5, result);
            Assert.Equal("New", employee.FirstName);
            Assert.Equal("Name", employee.LastName);
            Assert.Equal("new@company.com", employee.Email);
            Assert.Equal(3, employee.DepartmentId);
            Assert.Equal(35000, employee.Salary);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotDeleted()
        {
            _employeeRepoMock.Setup(r => r.DeleteAsync(4)).ReturnsAsync(false);

            var result = await _service.DeleteAsync(4);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenDeleted()
        {
            _employeeRepoMock.Setup(r => r.DeleteAsync(7)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.DeleteAsync(7);

            Assert.True(result);
        }

        [Fact]
        public async Task GetProjectsByEmployeeIdAsync_ReturnsProjectDtos()
        {
            var projects = new List<Project>
            {
                new Project { Id = 1, Name = "Alpha", ProjectCode = "A1", Budget = 10000 },
                new Project { Id = 2, Name = "Beta", ProjectCode = null, Budget = 20000 }
            };
            _projectRepoMock.Setup(r => r.GetProjectsByEmployeeIdAsync(3)).ReturnsAsync(projects);

            var result = await _service.GetProjectsByEmployeeIdAsync(3);

            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Name == "Alpha" && p.ProjectCode == "A1");
            Assert.Contains(result, p => p.Name == "Beta" && p.ProjectCode == string.Empty);
        }

        [Fact]
        public async Task GetAllAsync_ThrowsApplicationException_OnError()
        {
            _employeeRepoMock.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception("DB error"));

            await Assert.ThrowsAsync<ApplicationException>(() => _service.GetAllAsync());
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsApplicationException_OnError()
        {
            _employeeRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("DB error"));

            await Assert.ThrowsAsync<ApplicationException>(() => _service.GetByIdAsync(1));
        }

        [Fact]
        public async Task CreateAsync_ThrowsApplicationException_OnError()
        {
            _employeeRepoMock.Setup(r => r.AddAsync(It.IsAny<Employee>())).ThrowsAsync(new Exception("DB error"));

            var dto = new EmployeeCreateDto { FirstName = "Test", LastName = "User", Email = "test@company.com", Salary = 10000, DepartmentId = 1 };
            await Assert.ThrowsAsync<ApplicationException>(() => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_ThrowsApplicationException_OnError()
        {
            _employeeRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("DB error"));

            var dto = new EmployeeUpdateDto { FirstName = "Test", LastName = "User", Email = "test@company.com", DepartmentId = 1, Salary = 10000 };
            await Assert.ThrowsAsync<ApplicationException>(() => _service.UpdateAsync(1, dto));
        }

        [Fact]
        public async Task DeleteAsync_ThrowsApplicationException_OnError()
        {
            _employeeRepoMock.Setup(r => r.DeleteAsync(It.IsAny<int>())).ThrowsAsync(new Exception("DB error"));

            await Assert.ThrowsAsync<ApplicationException>(() => _service.DeleteAsync(1));
        }

        [Fact]
        public async Task GetProjectsByEmployeeIdAsync_ThrowsApplicationException_OnError()
        {
            _projectRepoMock.Setup(r => r.GetProjectsByEmployeeIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("DB error"));

            await Assert.ThrowsAsync<ApplicationException>(() => _service.GetProjectsByEmployeeIdAsync(1));
        }
    }
}