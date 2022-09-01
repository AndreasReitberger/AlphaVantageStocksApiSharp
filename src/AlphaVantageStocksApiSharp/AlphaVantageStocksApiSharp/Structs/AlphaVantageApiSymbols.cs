using System;

namespace AndreasReitberger.API.Structs
{
    public struct AlphaVantageApiSymbols
    {
        public static string None => "None";
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

    }
}
