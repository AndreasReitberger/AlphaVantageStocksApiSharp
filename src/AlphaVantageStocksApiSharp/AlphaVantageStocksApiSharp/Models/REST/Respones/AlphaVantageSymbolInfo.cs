using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Models.REST.Respones
{
    public partial class AlphaVantageSymbolInfo
    {
        #region Properties

        [JsonProperty("Symbol")]
        public string Symbol { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Exchange")]
        public string Exchange { get; set; }

        [JsonProperty("AssetType")]
        public string AssetType { get; set; }

        [JsonProperty("IpoDate")]
        public DateTime? IpoDate { get; set; }

        [JsonProperty("DelistingDate")]
        public DateTime? DelistingDate { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #endregion
    }
}
