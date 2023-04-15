using Newtonsoft.Json;

namespace StockPrice.Entities;

public class ExchangeRateData
{
    [JsonProperty("rates")]
    public Dictionary<string, double>? Rates { get; set; }
}