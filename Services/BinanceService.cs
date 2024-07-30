using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace APITask.Services;
public class BinanceService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl;

    public BinanceService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Binance:ApiKey"];
        _baseUrl = configuration["Binance:BaseUrl"];
    }

    public async Task<decimal> GetRateAsync(string symbol)
    {
        //async method HTTP GET to API, _baseUrl in appsettings.json
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v3/ticker/price?symbol={symbol}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var rate = JsonDocument.Parse(content).RootElement.GetProperty("price").GetDecimal();

        return rate;
    }
}

