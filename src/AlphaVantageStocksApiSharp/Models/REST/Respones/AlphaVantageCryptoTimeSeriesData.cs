using Newtonsoft.Json;

namespace AndreasReitberger.API.Models.REST.Respones
{
    public partial class AlphaVantageCryptoTimeSeriesData
    {
        #region Properties
        [JsonProperty("1a. open")]
        public double? OpenA { get; set; }
        [JsonProperty("1b. open")]
        public double? OpenB { get; set; }

        [JsonProperty("2a. high")]
        public double? HighA { get; set; }

        [JsonProperty("2b. high")]
        public double? HighB { get; set; }

        [JsonProperty("3a. low")]
        public double? LowA { get; set; }

        [JsonProperty("3b. low")]
        public double? LowB { get; set; }

        [JsonProperty("4a. close")]
        public double? CloseA { get; set; }

        [JsonProperty("4b. close")]
        public double? CloseB { get; set; }

        [JsonProperty("5. volume")]
        public double? Volume { get; set; }

        [JsonProperty("6. market cap")]
        public double? MarketCap { get; set; }

        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #endregion
    }
}
