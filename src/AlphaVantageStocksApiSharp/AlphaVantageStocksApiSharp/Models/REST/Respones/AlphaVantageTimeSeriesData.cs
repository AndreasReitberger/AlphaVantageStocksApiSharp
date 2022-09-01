using Newtonsoft.Json;

namespace AndreasReitberger.API.Models.REST.Respones
{
    public partial class AlphaVantageTimeSeriesData
    {
        #region Properties
        [JsonProperty("1. open")]
        public double? Open { get; set; }

        [JsonProperty("2. high")]
        public double? High { get; set; }

        [JsonProperty("3. low")]
        public double? Low { get; set; }

        [JsonProperty("4. close")]
        public double? Close { get; set; }

        [JsonProperty("5. volume")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long Volume { get; set; }

        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #endregion
    }
}
