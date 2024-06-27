using Blazored.LocalStorage;

namespace SensorSim.Client.Services;

public class TokenService
{
    private readonly ILocalStorageService _localStorageService;

    public TokenService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task SetToken(string username, string password)
    {
        await _localStorageService.SetItemAsync("token", $"Basic {Base64Encode($"{username}:{password}")}");
    }

    public async Task<string> GetToken()
    {
        return await _localStorageService.GetItemAsync<string>("token");
    }

    private string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    private string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}