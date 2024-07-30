using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace APITask.Services;
public class KuCoinService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl;

    public KuCoinService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["KuCoin:ApiKey"];
        _baseUrl = configuration["KuCoin:BaseUrl"];
    }

    public async Task<decimal> GetRateAsync(string symbol)
    {
        //async method HTTP GET to API, _baseUrl in appsettings.json
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/market/orderbook/level1?symbol={symbol}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var rate = JsonDocument.Parse(content).RootElement.GetProperty("data").GetProperty("price").GetDecimal();

        return rate;
    }
}

