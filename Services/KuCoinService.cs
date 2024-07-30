using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace APITask.Services;
public class KuCoinService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public KuCoinService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _baseUrl = "https://api.kucoin.com";
    }

    public async Task<decimal> GetRateAsync(string symbol)
    {
        //async method HTTP GET to API, _baseUrl in appsettings.
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/mark-price/{symbol}/current");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var rate = JsonDocument.Parse(content).RootElement.GetProperty("data").GetProperty("value").GetDecimal();
        //decimal.TryParse(rateString, out var rate);
        //calculation needed to get currency price
        rate = 1 / rate;
        rate = Math.Round(rate, 8);

        return rate;
    }
}

