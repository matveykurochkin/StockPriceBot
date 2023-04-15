using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using NLog;
using StockPrice.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StockPrice.Processors;

internal class GetExchangeRate
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    internal static async Task ExchangeRate(ITelegramBotClient botClient, Message message)
    {
        var baseCurrency = "USD";
        string[] targetCurrencies = { "RUB", "EUR", "CNY", "GBP" };

        try
        {
            var appId = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Tokens")["APIExchangeRateToken"];

            var stringBuilder = new StringBuilder();

            for (var i = 0; i < targetCurrencies.Length; i++)
            {
                if (i == targetCurrencies.Length - 1)
                    stringBuilder.Append(targetCurrencies[i]);
                else
                    stringBuilder.Append(targetCurrencies[i] + ",");
            }

            var url = $"https://openexchangerates.org/api/latest.json?app_id={appId}&base={baseCurrency}&symbols={stringBuilder}";
            _logger.Info($"Your URL: {url}");

            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ExchangeRateData>(content);

            if (data is not null && data.Rates!.TryGetValue(targetCurrencies[0], out var rate)
                                 && data.Rates.TryGetValue(targetCurrencies[1], out var rateE)
                                 && data.Rates.TryGetValue(targetCurrencies[2], out var rateC)
                                 && data.Rates.TryGetValue(targetCurrencies[3], out var rateG))
            {
                await botClient.SendTextMessageAsync(message.Chat, $"1 {baseCurrency} 🇺🇸 = {Math.Round(rate, 2)} {targetCurrencies[0]} 🇷🇺\n" +
                                                                   $"1 {targetCurrencies[1]} 🇪🇺 = {Math.Round(1 / rateE * rate, 2)} {targetCurrencies[0]} 🇷🇺\n" +
                                                                   $"1 {targetCurrencies[2]} 🇨🇳 = {Math.Round(1 / rateC * rate, 2)} {targetCurrencies[0]} 🇷🇺\n" +
                                                                   $"1 {targetCurrencies[3]} 🇬🇧 = {Math.Round(1 / rateG * rate, 2)} {targetCurrencies[0]} 🇷🇺");
            }
            else
                await botClient.SendTextMessageAsync(message.Chat, $"Произошла ошибка. Попробуйте запросить курс валют позже!");
        }
        catch (Exception ex)
        {
            _logger.Error($"Error on Exchange Rate. Error message: {ex.Message}");
        }
    }
}