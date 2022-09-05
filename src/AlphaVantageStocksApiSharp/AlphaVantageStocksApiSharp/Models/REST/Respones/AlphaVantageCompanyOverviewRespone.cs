using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Models.REST.Respones
{
    public partial class AlphaVantageCompanyOverviewRespone
    {
        #region Properties
        [JsonProperty("Symbol")]
        public string Symbol { get; set; }

        [JsonProperty("AssetType")]
        public string AssetType { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("CIK")]
        public long Cik { get; set; }

        [JsonProperty("Exchange")]
        public string Exchange { get; set; }

        [JsonProperty("Currency")]
        public string Currency { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("Sector")]
        public string Sector { get; set; }

        [JsonProperty("Industry")]
        public string Industry { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("FiscalYearEnd")]
        public string FiscalYearEnd { get; set; }

        [JsonProperty("LatestQuarter")]
        public DateTimeOffset LatestQuarter { get; set; }

        [JsonProperty("MarketCapitalization")]
        public long? MarketCapitalization { get; set; }

        [JsonProperty("EBITDA")]
        public long? Ebitda { get; set; }

        [JsonProperty("PERatio")]
        public double? PeRatio { get; set; }

        [JsonProperty("PEGRatio")]
        public double? PegRatio { get; set; }

        [JsonProperty("BookValue")]
        public double? BookValue { get; set; }

        [JsonProperty("DividendPerShare")]
        public double? DividendPerShare { get; set; }

        [JsonProperty("DividendYield")]
        public double? DividendYield { get; set; }

        [JsonProperty("EPS")]
        public double? Eps { get; set; }

        [JsonProperty("RevenuePerShareTTM")]
        public double? RevenuePerShareTtm { get; set; }

        [JsonProperty("ProfitMargin")]
        public double? ProfitMargin { get; set; }

        [JsonProperty("OperatingMarginTTM")]
        public double? OperatingMarginTtm { get; set; }

        [JsonProperty("ReturnOnAssetsTTM")]
        public double? ReturnOnAssetsTtm { get; set; }

        [JsonProperty("ReturnOnEquityTTM")]
        public double? ReturnOnEquityTtm { get; set; }

        [JsonProperty("RevenueTTM")]
        public long? RevenueTtm { get; set; }

        [JsonProperty("GrossProfitTTM")]
        public long? GrossProfitTtm { get; set; }

        [JsonProperty("DilutedEPSTTM")]
        public double? DilutedEpsttm { get; set; }

        [JsonProperty("QuarterlyEarningsGrowthYOY")]
        public double? QuarterlyEarningsGrowthYoy { get; set; }

        [JsonProperty("QuarterlyRevenueGrowthYOY")]
        public double? QuarterlyRevenueGrowthYoy { get; set; }

        [JsonProperty("AnalystTargetPrice")]
        public double? AnalystTargetPrice { get; set; }

        [JsonProperty("TrailingPE")]
        public double? TrailingPe { get; set; }

        [JsonProperty("ForwardPE")]
        public double? ForwardPe { get; set; }

        [JsonProperty("PriceToSalesRatioTTM")]
        public double? PriceToSalesRatioTtm { get; set; }

        [JsonProperty("PriceToBookRatio")]
        public double? PriceToBookRatio { get; set; }

        [JsonProperty("EVToRevenue")]
        public double? EvToRevenue { get; set; }

        [JsonProperty("EVToEBITDA")]
        public double? EvToEbitda { get; set; }

        [JsonProperty("Beta")]
        public double? Beta { get; set; }

        [JsonProperty("52WeekHigh")]
        public double? The52WeekHigh { get; set; }

        [JsonProperty("52WeekLow")]
        public double? The52WeekLow { get; set; }

        [JsonProperty("50DayMovingAverage")]
        public double? The50DayMovingAverage { get; set; }

        [JsonProperty("200DayMovingAverage")]
        public double? The200DayMovingAverage { get; set; }

        [JsonProperty("SharesOutstanding")]
        public long SharesOutstanding { get; set; }

        [JsonProperty("DividendDate")]
        public DateTimeOffset DividendDate { get; set; }

        [JsonProperty("ExDividendDate")]
        public DateTimeOffset ExDividendDate { get; set; }

        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #endregion
    }
}
