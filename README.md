# [StockPriceBot](https://t.me/InvestPriceBot) для Телеграма на C#
Данный бот используется для отслеживания цены самых популярных акций по их тикерам (по нажатию кнопки или написав тикер самостоятельно) и просмотра курса валют. 

## Подключение данных
* Данные о цене каждой акции по ее тикеру берутся из API сервиса [API Alpha Vantage](https://www.alphavantage.co/)

* Данные о курсе валют берутся из API сервиса [Open Exchange Rate](https://openexchangerates.org/)

## Как запустить бота
1. Введите токен бота (его можно получить у [Bot Father](https://t.me/BotFather)) в файл `appsettings.json` в поле `TelegramBotToken`
2. Вставьте ваш ключ API Alpha Vantage в файле `appsettings.json` в поле `APIStockPricesToken`
3. Вставьте ваш ключ API Open Exchange Rate  в файле `appsettings.json` в поле `APIExchangeRateToken`

## Создание службы
```bash
sc.exe create "Name" binpath="Path to your exe" start="demand"
```
## Удаление службы
```bash
sc.exe delete "Name"
```