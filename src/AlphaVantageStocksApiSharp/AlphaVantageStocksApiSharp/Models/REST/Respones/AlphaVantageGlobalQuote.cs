using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Models.REST.Respones
{
    public partial class AlphaVantageGlobalQuote
    {
        #region Properties
        [JsonProperty("01. symbol")]
        public string Symbol { get; set; }

        [JsonProperty("02. open")]
        public double? Open { get; set; }

        [JsonProperty("03. high")]
        public double? High { get; set; }

        [JsonProperty("04. low")]
        public double? Low { get; set; }

        [JsonProperty("05. price")]
        public double? Price { get; set; }

        [JsonProperty("06. volume")]
        public long? Volume { get; set; }

        [JsonProperty("07. latest trading day")]
        public DateTimeOffset LatestTradingDay { get; set; }

        [JsonProperty("08. previous close")]
        public double? PreviousClose { get; set; }

        [JsonProperty("09. change")]
        public double? Change { get; set; }

        [JsonProperty("10. change percent")]
        public double? ChangePercent { get; set; }

        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #endregion
    }
}
