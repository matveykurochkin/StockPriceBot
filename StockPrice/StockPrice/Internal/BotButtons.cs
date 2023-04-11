using Telegram.Bot.Types.ReplyMarkups;

namespace StockPrice.Internal
{
    static internal class BotButtons
    {
        public static IReplyMarkup MainButtonOnBot()
        {
            var tgButton = new ReplyKeyboardMarkup(new[]
            {
        new[]
        {
            new KeyboardButton("Список популярных акций 💵"),
        }
         });
            tgButton.ResizeKeyboard = true;
            return tgButton;
        }
        public static IReplyMarkup MostPopularStock()
        {
            var tgButton = new ReplyKeyboardMarkup(new[]
            {
        new[]
        {
            new KeyboardButton("AAPL"),
            new KeyboardButton("YNDX"),
            new KeyboardButton("TSLA")
        },
        new[]
        {
            new KeyboardButton("Назад ⬅")
        }
         });
            tgButton.ResizeKeyboard = true;
            return tgButton;
        }
    }
}
