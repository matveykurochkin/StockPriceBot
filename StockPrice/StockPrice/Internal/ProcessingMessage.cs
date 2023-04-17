using NLog;
using StockPrice.Processors;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StockPrice.Internal;

internal class ProcessingMessage
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var message = update.Message;

            _logger.Info($"Пользователь {message?.From?.FirstName} {message?.From?.LastName} написал боту данное сообщение: {message?.Text}\n id Пользователя: {message?.From?.Id}");

            if (message?.Text is not null)
            {
                if (message?.Text == "/start" || message?.Text == "Назад ⬅")
                {
                    await botClient.SendTextMessageAsync(message.Chat, $"Мои возможности!" +
                                                                       $"\nНапиши тикер нужной акции и узнай ее текущую цену!" +
                                                                       $"\nТакже можешь воспользоваться кнопкой \"Популярные акции 💵\" или нажать сюда: /listmostpopularstock" +
                                                                       $"\nМожешь узнать курс авлют нажав на кнопку \"Курс валют 💶\" или нажать сюда: /exchangerate", replyMarkup: BotButtons.MainButtonOnBot(), cancellationToken: cancellationToken);
                    return;
                }

                if (message?.Text == "Популярные акции 💵" || message?.Text == "/listmostpopularstock")
                {
                    await botClient.SendTextMessageAsync(message.Chat, $"Держи список популярных акций!", replyMarkup: BotButtons.MostPopularStock(), cancellationToken: cancellationToken);
                    return;
                }

                if (message?.Text == "Курс валют 💶" || message?.Text == "/exchangerate")
                {
                    await GetExchangeRate.ExchangeRate(botClient, message!);
                    return;
                }
                else
                {
                    await GetStockPrice.StockPrice(botClient, message!, message!.Text);
                    return;
                }
            }
        }
    }

    public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.Error(exception, "Error received in telegram bot");
        return Task.CompletedTask;
    }
}