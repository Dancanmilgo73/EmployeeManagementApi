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
        try
        {
            var payload = new
            {
                codesToGenerate = 1,
                onlyUniques = true,
                prefix = "CY-",
                charactersSets = new[] { "ABCDE", "FGHIJ", "KLMNO", "PQRST", "UVWXY", "Z", "\\d" },
            };

            var json = System.Text.Json.JsonSerializer.Serialize(payload);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://codito.io/free-random-code-generator/api/generate", content);
            var result = await response.Content.ReadAsStringAsync();
            var code = System.Text.Json.JsonSerializer.Deserialize<string[]>(result)?[0] ?? string.Empty;
            return code;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred creating a project code.", ex);
        }
    }
}