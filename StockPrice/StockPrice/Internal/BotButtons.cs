using Telegram.Bot.Types.ReplyMarkups;

namespace StockPrice.Internal;
static internal class BotButtons
{
    public static IReplyMarkup MainButtonOnBot()
    {
        var tgButton = new ReplyKeyboardMarkup(new[]
        {
    new[]
    {
        new KeyboardButton("Популярные акции 💵"),
        new KeyboardButton("Курс валют 💶")
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
        new KeyboardButton("MSFT"),
        new KeyboardButton("TSLA"),
        new KeyboardButton("GOOG")
    },
    new[]
    {
        new KeyboardButton("NVDA"),
        new KeyboardButton("META"),
        new KeyboardButton("MA"),
        new KeyboardButton("MCD")
    },
    new[]
    {
        new KeyboardButton("ADBE"),
        new KeyboardButton("STEM"),
        new KeyboardButton("INTC"),
        new KeyboardButton("PEP")
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