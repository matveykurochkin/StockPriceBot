using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StockPrice.Processors;
static internal class GetStockPrice
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    static internal async Task StockPrice(ITelegramBotClient botClient, Message message, string textMessage)
    {
        var apiKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Tokens")["APIStockPricesToken"];

        string url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={textMessage}&apikey={apiKey}";

        HttpClient client = new HttpClient();

        HttpResponseMessage response = await client.GetAsync(url);
        string jsonString = await response.Content.ReadAsStringAsync();
        dynamic result = JsonConvert.DeserializeObject(jsonString);

        if (result != null && result!["Global Quote"] != null && result!["Global Quote"]["05. price"] != null)
        {
            await botClient.SendTextMessageAsync(message.Chat, $"Информация об акции {textMessage}!\n" +
                $"Текущая цена акции: {Math.Round(Convert.ToDouble(result!["Global Quote"]["05. price"]), 2)} $\n" +
                $"Цена открытия: {Math.Round(Convert.ToDouble(result!["Global Quote"]["02. open"]), 2)} $\n" +
                $"Максимальная цена: {Math.Round(Convert.ToDouble(result!["Global Quote"]["03. high"]), 2)} $\n" +
                $"Минимальная цена: {Math.Round(Convert.ToDouble(result!["Global Quote"]["04. low"]), 2)} $\n" +
                $"Количество акций, которые торговались в течение дня: {Math.Round(Convert.ToDouble(result!["Global Quote"]["06. volume"]), 2).ToString("#,#", new CultureInfo("ru-RU"))}\n" +
                $"Цена закрытия в предыдущий торговый день: {Math.Round(Convert.ToDouble(result!["Global Quote"]["08. previous close"]), 2)} $\n" +
                $"Изменение цены: {Math.Round(Convert.ToDouble(result!["Global Quote"]["09. change"]), 2)} $\n");
            _logger.Info($"Info for stock {textMessage} successfully received!");
        }
        else
        {
            await botClient.SendTextMessageAsync(message.Chat, $"Данная акция не найдена или исчерпан лимит запросов!");
            _logger.Info($"This stock not been found or the request limit been reached!");
        }
        return;
    }
}