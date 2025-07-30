using System.Threading.Tasks;

namespace EmployeeManagementApi.Application.Interfaces;
public interface IRandomStringGeneratorService
{
    Task<string> GenerateRandomStringAsync(int length);
}