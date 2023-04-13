using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StockPrice.Processors;
static internal class GetExchangeRate
{
    static internal async Task ExchangeRate(ITelegramBotClient botClient, Message message)
    {
        string baseCurrency = "USD";
        string[] targetCurrencies = { "RUB", "EUR" };

        string? appId = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("APIExchangeRate")["APIExchangeRateToken"];

        string url = $"https://openexchangerates.org/api/latest.json?app_id={appId}&base={baseCurrency}";
        Console.WriteLine(url);

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            ExchangeRateData data = JsonConvert.DeserializeObject<ExchangeRateData>(content);

            if (data is not null && data.Rates!.TryGetValue(targetCurrencies[0], out double rate) && data.Rates.TryGetValue(targetCurrencies[1], out double rateE))
            {
                await botClient.SendTextMessageAsync(message.Chat, $"1 {baseCurrency} 🇺🇸 = {Math.Round(rate, 2)} {targetCurrencies[0]} 🇷🇺\n" +
                    $"1 {targetCurrencies[1]} 🇪🇺 = {Math.Round(1 / rateE * rate, 2)} {targetCurrencies[0]} 🇷🇺");
            }
            else
                await botClient.SendTextMessageAsync(message.Chat, $"Произошла ошибка. Попробуйте запросить курс валют позже!");
            return;
        }
    }
}