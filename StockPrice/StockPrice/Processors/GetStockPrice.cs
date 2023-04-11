﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StockPrice.Processors
{
    static internal class GetStockPrice
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        static internal async Task StockPrice(ITelegramBotClient botClient, Message message, string textMessage)
        {

            var apiKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("APIStockPrices")["APIToken"];

            string url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={textMessage}&apikey={apiKey}";


            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(url);
            string jsonString = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(jsonString);

            if (result != null && result!["Global Quote"] != null && result!["Global Quote"]["05. price"] != null)
            {
                await botClient.SendTextMessageAsync(message.Chat, $"Цена акции {textMessage}: {Math.Round(Convert.ToDouble(result!["Global Quote"]["05. price"]), 2)} $");
                _logger.Info($"Price of {textMessage} is {result["Global Quote"]["05. price"]} $");
                return;
            }
            else
            {
                await botClient.SendTextMessageAsync(message.Chat, $"Данная акция не найдена или исчерпан лимит запросов!");
                return;
            }

        }
    }
}
