using Telegram.Bot.Types.ReplyMarkups;

namespace StockPrice.Internal;

internal static class BotButtons
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
        })
        {
            ResizeKeyboard = true
        };
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
                new KeyboardButton("NFLX"),
                new KeyboardButton("V"),
                new KeyboardButton("PYPL"),
                new KeyboardButton("AMZN")
            },
            new[]
            {
                new KeyboardButton("AMD"),
                new KeyboardButton("DIS"),
                new KeyboardButton("BABA"),
                new KeyboardButton("NKE")
            },
            new[]
            {
                new KeyboardButton("Назад ⬅")
            }
        })
        {
            ResizeKeyboard = true
        };
        return tgButton;
    }
}