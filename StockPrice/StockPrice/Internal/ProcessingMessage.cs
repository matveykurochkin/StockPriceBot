using StockPrice.Processors;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StockPrice.Internal;
internal class ProcessingMessage: IProcessing
{
    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var message = update.Message;

            IProcessing._logger.Info($"Пользователь {message?.From?.FirstName} {message?.From?.LastName} написал боту данное сообщение: {message?.Text}\n id Пользователя: {message?.From?.Id}");

            if (message?.Text is not null)
            {
                if (message?.Text == "/start" || message?.Text == "Назад ⬅")
                {
                    await botClient.SendTextMessageAsync(message.Chat, $"Мои возможности!" +
                        $"\nНапиши тикер нужной акции и узнай ее текущую цену!" +
                        $"\nТакже можешь воспользоваться кнопкой \"Популярные акции 💵\" или нажать сюда: /listmostpopularstock", replyMarkup: BotButtons.MainButtonOnBot());
                    return;
                }
                else if (message?.Text == "Популярные акции 💵" || message?.Text == "/listmostpopularstock")
                {
                    await botClient.SendTextMessageAsync(message.Chat, $"Держи список популярных акций!", replyMarkup: BotButtons.MostPopularStock());
                    return;
                }
                else if (message?.Text == "Курс валют 💶" || message?.Text == "/exchangerate")
                {
                    await GetExchangeRate.ExchangeRate(botClient, message!);
                }
                else
                {
                    await GetStockPrice.StockPrice(botClient, message!, message!.Text);
                }
            }
        }
    }

    public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        IProcessing._logger.Error(exception, "Error received in telegram bot");
        return Task.CompletedTask;
    }
}