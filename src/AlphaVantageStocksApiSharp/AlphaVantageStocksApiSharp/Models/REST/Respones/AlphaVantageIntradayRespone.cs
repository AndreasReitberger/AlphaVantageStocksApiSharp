using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Models.REST.Respones
{
    public class AlphaVantageIntradayRespone
    {
        #region Properties
        [JsonProperty("Meta Data")]
        public AlphaVantageMetaData MetaData { get; set; }

        [JsonProperty("Time Series")]
        public Dictionary<string, AlphaVantageTimeSeriesData> TimeSeries { get; set; } = new();

        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #endregion
    }
}
