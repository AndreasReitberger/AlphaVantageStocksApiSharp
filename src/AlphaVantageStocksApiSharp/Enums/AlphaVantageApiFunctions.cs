﻿namespace AndreasReitberger.API.Enums
{
    public enum AlphaVantageApiFunctions
    {
        // Core Stocks API
        LISTING_STATUS,
        SYMBOL_SEARCH,
        TIME_SERIES_INTRADAY,
        TIME_SERIES_INTRADAY_EXTENDED,
        TIME_SERIES_DAILY,
        TIME_SERIES_DAILY_ADJUSTED,     // Premium
        TIME_SERIES_WEEKLY,
        TIME_SERIES_WEEKLY_ADJUSTED,
        TIME_SERIES_MONTHLY,
        TIME_SERIES_MONTHLY_ADJUSTED,
        GLOBAL_QUOTE,

        // Fundamental Data
        OVERVIEW,

        // Crypto
        CRYPTO_INTRADAY,
        DIGITAL_CURRENCY_DAILY,
        DIGITAL_CURRENCY_WEEKLY,
        DIGITAL_CURRENCY_MONTHLY,
    }
}
