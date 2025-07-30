using EmployeeManagementApi.Application.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace EmployeeManagementApi.Application.Services;
public class RandomStringGeneratorService : IRandomStringGeneratorService
{
    private readonly HttpClient _httpClient;
    public RandomStringGeneratorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<string> GenerateRandomStringAsync(int length)
    {
        var response = await _httpClient.GetStringAsync($"https://codito.io/free-random-code-generator/api/generate");
        return response; 
    }
}