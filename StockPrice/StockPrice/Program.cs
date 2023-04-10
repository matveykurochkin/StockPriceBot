using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using StockPrice.Processors;
using NLog;
using Microsoft.Extensions.Configuration;

namespace TelegramBotExperiments
{
    class Program
    {
        private static string? token = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("TelegramBotToken")["Token"];
       
        private static readonly ITelegramBotClient telegramBot = new TelegramBotClient(token!);

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {          
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;

                _logger.Info($"Пользователь {message?.From?.FirstName} {message?.From?.LastName} написал боту данное сообщение: {message?.Text}\n id Пользователя: {message?.From?.Id}");

                if (message?.Text is not null)
                {
                    await GetStockPrice.StockPrice(botClient, message, message.Text);
                    return;
                }
            }
        }

        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            _logger.Error(exception, "Error received in telegram bot");
            return Task.CompletedTask;
        }

        static void Main(string[] args)
        {
            try
            {
                _logger.Info($"Бот {telegramBot.GetMeAsync().Result.FirstName} успешно запущен!");

                var cts = new CancellationTokenSource();
                var cancellationToken = cts.Token;
                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = { },
                };
                telegramBot.StartReceiving(
                    HandleUpdateAsync,
                    HandleErrorAsync,
                    receiverOptions,
                    cancellationToken
                );
                Console.ReadLine();
            }catch (Exception ex)
            {
                _logger.Error($"Error message: {ex.Message}");
            }
        }
    }
}