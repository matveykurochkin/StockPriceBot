using Telegram.Bot;
using Telegram.Bot.Polling;
using StockPrice.Internal;

namespace TelegramBotExperiments;
class Program : IProcessing
{
    static void Main()
    {
        try
        {
            IProcessing._logger.Info($"Бот {IProcessing.telegramBot.GetMeAsync().Result.FirstName} успешно запущен!");
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
            };
            IProcessing.telegramBot.StartReceiving(
               ProcessingMessage.HandleUpdateAsync,
                ProcessingMessage.HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }catch (Exception ex)
        {
            IProcessing._logger.Error($"Error message: {ex.Message}");
        }
    }
}