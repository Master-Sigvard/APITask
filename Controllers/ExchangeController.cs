using APITask.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class ExchangeController : ControllerBase
{
    private readonly BinanceService _binanceService;
    private readonly KuCoinService _kucoinService;

    public ExchangeController(BinanceService binanceService, KuCoinService kucoinService)
    {
        _binanceService = binanceService;
        _kucoinService = kucoinService;
    }

    [HttpGet("estimate")]
    public async Task<IActionResult> Estimate(decimal inputAmount, string inputCurrency, string outputCurrency)
    {
        //exchange rate
        var binanceRate = await _binanceService.GetRateAsync($"{inputCurrency}{outputCurrency}");
        /* Kucoin api has different supported symbols from binance api, (USDTBTC for binance and BTC-USDT for Kucoin)
         * so in next line input and output currencies was reversed to get proper GET request to kucoin api
         and then in KuCoinService the required price is calculated based on the received */
        var kucoinRate = await _kucoinService.GetRateAsync($"{outputCurrency}-{inputCurrency}");

        //exchange result
        var binanceOutput = inputAmount * binanceRate;
        var kucoinOutput = inputAmount * kucoinRate;

        var bestExchange = binanceOutput > kucoinOutput ? "Binance" : "KuCoin";
        var bestAmount = binanceOutput > kucoinOutput ? binanceOutput : kucoinOutput;

        return Ok(new { exchangeName = bestExchange, outputAmount = bestAmount });
    }

    [HttpGet("getRates")]
    public async Task<IActionResult> GetRates(string baseCurrency, string quoteCurrency)
    {
        var binanceRate = await _binanceService.GetRateAsync($"{baseCurrency}{quoteCurrency}");
        /* Kucoin api has different supported symbols from binance api, (USDTBTC for binance and BTC-USDT for Kucoin)
        * so in next line input and output currencies was reversed to get proper GET request to kucoin api
        and then in KuCoinService the required price is calculated based on the received */
        var kucoinRate = await _kucoinService.GetRateAsync($"{quoteCurrency}-{baseCurrency}");

        return Ok(new[]
        {
            new { exchangeName = "Binance", rate = binanceRate },
            new { exchangeName = "KuCoin", rate = kucoinRate }
        });
    }
}

