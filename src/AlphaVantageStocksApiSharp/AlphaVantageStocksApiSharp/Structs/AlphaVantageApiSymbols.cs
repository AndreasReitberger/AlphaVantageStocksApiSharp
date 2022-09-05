using System;

namespace AndreasReitberger.API.Structs
{
    public struct AlphaVantageApiSymbols
    {
        // Note, this is not the full list. Expand this struct for missing symbols
        public static string None => "None";

        #region Core Stocks API

        // DOW
        public static string AppleInc => "AAPL";  // Apple Inc
        public static string DowInc => "DOW";   // Dow Inc
        public static string IBM => "IBM";   // IBM
        // DAX
        [Obsolete("Use MercedesBenzGroupAG instead")]
        public static string DaimlerAG => "DAI.DEX";
        public static string MercedesBenzGroupAG => "MBG.DEX";
        public static string BASF => "BAS.DEX";
        public static string EON => "EOAN.DEX";
        #endregion

        #region CryptoCurrencies
        public static string Bitcoin => "BTC";
        public static string Etherum => "ETH";
        public static string EtherumClassic => "ETC";
        #endregion
    }
}
