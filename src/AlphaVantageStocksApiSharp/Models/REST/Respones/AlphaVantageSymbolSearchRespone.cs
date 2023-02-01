using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Models.REST.Respones
{
    public class AlphaVantageSymbolSearchRespone
    {
        #region Properties

        [JsonProperty("bestMatches")]
        public List<AlphaVantageSymbolSearchMatch> BestMatches { get; set; } = new();

        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #endregion
    }
}
