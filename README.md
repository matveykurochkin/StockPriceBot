# [StockPriceBot](https://t.me/InvestPriceBot) для Телеграма на C#
Данный бот используется для отслеживания цены акций по их тикерам.  

## Подключение данных
* Данные о цене каждой акции по ее тикеру берутся из API сервиса [API Alpha Vantage](https://www.alphavantage.co/)

* Данные о курсе валют берутся из API сервиса [Open Exchange Rate](https://openexchangerates.org/)

## Как запустить бота
1. Введите токен бота (его можно получить у [Bot Father](https://t.me/BotFather)) в файл `appsettings.json` в поле `Token`
2. Вставьте ваш ключ API Alpha Vantage в файле `appsettings.json` в поле `APIToken`
3. Вставьте ваш ключ API Open Exchange Rate  в файле `appsettings.json` в поле `APIExchangeRateToken`