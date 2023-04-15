using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StockPrice.Internal;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StockPrice.Processors;
internal class GetExchangeRate : IProcessing
{
    static internal async Task ExchangeRate(ITelegramBotClient botClient, Message message)
    {
        string baseCurrency = "USD";
        string[] targetCurrencies = { "RUB", "EUR", "CNY", "GBP" };

        try
        {
            string? appId = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Tokens")["APIExchangeRateToken"];

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < targetCurrencies.Length; i++)
            {
                if (i == targetCurrencies.Length - 1)
                    stringBuilder.Append(targetCurrencies[i]);
                else
                    stringBuilder.Append(targetCurrencies[i] + ",");
            }

            string url = $"https://openexchangerates.org/api/latest.json?app_id={appId}&base={baseCurrency}&symbols={stringBuilder}";
            IProcessing._logger.Info($"Your URL: {url}");

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                string content = await response.Content.ReadAsStringAsync();
                ExchangeRateData data = JsonConvert.DeserializeObject<ExchangeRateData>(content);

                if (data is not null && data.Rates!.TryGetValue(targetCurrencies[0], out double rate)
                    && data.Rates.TryGetValue(targetCurrencies[1], out double rateE)
                    && data.Rates.TryGetValue(targetCurrencies[2], out double rateC)
                    && data.Rates.TryGetValue(targetCurrencies[3], out double rateG))
                {
                    await botClient.SendTextMessageAsync(message.Chat, $"1 {baseCurrency} 🇺🇸 = {Math.Round(rate, 2)} {targetCurrencies[0]} 🇷🇺\n" +
                        $"1 {targetCurrencies[1]} 🇪🇺 = {Math.Round(1 / rateE * rate, 2)} {targetCurrencies[0]} 🇷🇺\n" +
                        $"1 {targetCurrencies[2]} 🇨🇳 = {Math.Round(1 / rateC * rate, 2)} {targetCurrencies[0]} 🇷🇺\n" +
                        $"1 {targetCurrencies[3]} 🇬🇧 = {Math.Round(1 / rateG * rate, 2)} {targetCurrencies[0]} 🇷🇺");
                }
                else
                    await botClient.SendTextMessageAsync(message.Chat, $"Произошла ошибка. Попробуйте запросить курс валют позже!");
                return;
            }
        }
        catch (Exception ex)
        {
            IProcessing._logger.Error($"Error on Exchange Rate. Error message: {ex.Message}");
        }
    }
}