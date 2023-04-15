using Newtonsoft.Json;

namespace StockPrice.Processors;

public class ExchangeRateData
{
    [JsonProperty("rates")]
    public Dictionary<string, double>? Rates { get; set; }
}