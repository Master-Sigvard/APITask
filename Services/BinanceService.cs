using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace APITask.Services;
public class BinanceService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public BinanceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _baseUrl = "https://api.binance.com";
    }

    public async Task<decimal> GetRateAsync(string symbol)
    {
        //async method HTTP GET to API, _baseUrl in appsettings.json
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v3/ticker/price?symbol={symbol}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var rateString = JsonDocument.Parse(content).RootElement.GetProperty("price").GetString();
        decimal.TryParse(rateString, out var rate);

        return rate;
    }
}

