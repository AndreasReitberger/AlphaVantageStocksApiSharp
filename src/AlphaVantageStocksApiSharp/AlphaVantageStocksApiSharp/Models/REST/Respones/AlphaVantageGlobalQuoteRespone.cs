using Newtonsoft.Json;

namespace AndreasReitberger.API.Models.REST.Respones
{
    public partial class AlphaVantageGlobalQuoteRespone
    {
        #region Properties
        [JsonProperty("Global Quote")]
        public AlphaVantageGlobalQuote GlobalQuote { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #endregion
    }
}
