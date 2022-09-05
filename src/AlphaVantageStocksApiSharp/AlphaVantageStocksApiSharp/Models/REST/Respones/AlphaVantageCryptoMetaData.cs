using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Models.REST.Respones
{
    public partial class AlphaVantageCryptoMetaData
    {
        #region Properties
        [JsonProperty("1. Information")]
        public string Information { get; set; }

        [JsonProperty("2. Digital Currency Code")]
        public string DigitalCurrencyCode { get; set; }

        [JsonProperty("3. Digital Currency Name")]
        public string DigitalCurrencyName { get; set; }

        [JsonProperty("4. Market Code")]
        public string MarketCode { get; set; }

        [JsonProperty("5. Market Name")]
        public string MarketName { get; set; }

        [JsonProperty("6. Last Refreshed")]
        public DateTimeOffset LastRefreshed { get; set; }

        [JsonProperty("7. Interval")]
        public string Interval { get; set; }

        [JsonProperty("8. Output Size")]
        public string OutputSize { get; set; }

        [JsonProperty("9. Time Zone")]
        public string TimeZone { get; set; }
    
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #endregion
    }
}
