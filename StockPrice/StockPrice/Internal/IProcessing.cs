using Microsoft.Extensions.Configuration;
using NLog;
using Telegram.Bot;

namespace StockPrice.Internal;
public interface IProcessing
{
    public static string? token = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Tokens")["TelegramBotToken"];

    public static readonly ITelegramBotClient telegramBot = new TelegramBotClient(token!);

    public static readonly Logger _logger = LogManager.GetCurrentClassLogger();
}