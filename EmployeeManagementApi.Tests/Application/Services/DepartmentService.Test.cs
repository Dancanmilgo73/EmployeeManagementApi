using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementApi.Application.Services;
using EmployeeManagementApi.Infrastructure.Repositories.Interfaces;
using EmployeeManagementApi.Models.DTOs;
using EmployeeManagementApi.Models.Entities;
using Moq;
using Xunit;

namespace EmployeeManagementApi.Application.Services.Tests
{
    public class DepartmentServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IDepartmentRepository> _departmentRepoMock;
        private readonly DepartmentService _service;

        public DepartmentServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _departmentRepoMock = new Mock<IDepartmentRepository>();
            _unitOfWorkMock.SetupGet(u => u.Departments).Returns(_departmentRepoMock.Object);
            _service = new DepartmentService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsDepartmentDtos()
        {
            var departments = new List<Department>
            {
                new Department { Id = 1, Name = "HR", OfficeLocation = "A1" },
                new Department { Id = 2, Name = "IT", OfficeLocation = "B2" }
            };
            _departmentRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(departments);

            var result = await _service.GetAllAsync();

            Assert.Equal(2, result.Count());
            Assert.Contains(result, d => d.Name == "HR" && d.Location == "A1");
            Assert.Contains(result, d => d.Name == "IT" && d.Location == "B2");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsDepartmentDto_WhenFound()
        {
            var department = new Department { Id = 1, Name = "HR", OfficeLocation = "LARNACA" };
            _departmentRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(department);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("HR", result.Name);
            Assert.Equal("LARNACA", result.Location);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            _departmentRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Department?)null);

            var result = await _service.GetByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_AddsDepartmentAndReturnsId()
        {
            var dto = new DepartmentCreateDto { Name = "Finance", Location = "KENYA" };
            var department = new Department { Id = 10, Name = "Finance", OfficeLocation = "KENYA" };

            _departmentRepoMock.Setup(r => r.AddAsync(It.IsAny<Department>()))
                .Callback<Department>(d => d.Id = department.Id)
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.CreateAsync(dto);

            Assert.Equal(10, result);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenDepartmentNotFound()
        {
            _departmentRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Department?)null);

            var dto = new DepartmentUpdateDto { Name = "Any", Location = "Any" };
            var result = await _service.UpdateAsync(99, dto);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotDeleted()
        {
            _departmentRepoMock.Setup(r => r.DeleteAsync(4)).ReturnsAsync(false);

            var result = await _service.DeleteAsync(4);

            Assert.False(result);
        }

        [Fact]
        public async Task GetAllAsync_ThrowsApplicationException_OnError()
        {
            _departmentRepoMock.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception("DB error"));

            await Assert.ThrowsAsync<ApplicationException>(() => _service.GetAllAsync());
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsApplicationException_OnError()
        {
            _departmentRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("DB error"));

            await Assert.ThrowsAsync<ApplicationException>(() => _service.GetByIdAsync(1));
        }

        [Fact]
        public async Task CreateAsync_ThrowsApplicationException_OnError()
        {
            _departmentRepoMock.Setup(r => r.AddAsync(It.IsAny<Department>())).ThrowsAsync(new Exception("DB error"));

            var dto = new DepartmentCreateDto { Name = "Finance", Location = "C3" };
            await Assert.ThrowsAsync<ApplicationException>(() => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_ThrowsApplicationException_OnError()
        {
            _departmentRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("DB error"));

            var dto = new DepartmentUpdateDto { Name = "ANALYTICS DEPT", Location = "CYPRUS" };
            await Assert.ThrowsAsync<ApplicationException>(() => _service.UpdateAsync(1, dto));
        }

        [Fact]
        public async Task DeleteAsync_ThrowsApplicationException_OnError()
        {
            _departmentRepoMock.Setup(r => r.DeleteAsync(It.IsAny<int>())).ThrowsAsync(new Exception("DB error"));

            await Assert.ThrowsAsync<ApplicationException>(() => _service.DeleteAsync(1));
        }
    }
}