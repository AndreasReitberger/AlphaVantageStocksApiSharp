# AlphaVantageStocksApiSharp
A C# wrapper around the Alpha Vantage Stocks API (https://www.alphavantage.co/documentation/)

# Usage

## Create a client
In order to create a `AlphaVantageClient` you either can create a new instance of it, or yous the `AlphaVantageConnectionBuilder`.

```cs 
const string web = "https://www.alphavantage.co"; // This is the default address, you do not need to set this
const string api = "demo";
        
 AlphaVantageClient client = new AlphaVantageClient.AlphaVantageConnectionBuilder()
  .WithWebAddressAndApiKey(webAddress: web, apiKey: api)
  .Build();
// Make sure that you check if the server is online first, otherwise no requests will work!
await client.CheckOnlineAsync();
```

## Search symbols
The api works with symbols. Most known stocks symbols are available with the `AlphaVantageApiSymbols` struct.
However, you also can search the api.

```cs
AlphaVantageSymbolSearchRespone symbols = await client.SearchSymbolsAsync("Merc");
var matches = symbols.BestMatches; // Holds the 10 best matches according to the provided keyword
```
For missing default symbols, you can commit to the `AlphaVantageApiSymbols` struct file.
- https://github.com/AndreasReitberger/AlphaVantageStocksApiSharp/blob/main/src/AlphaVantageStocksApiSharp/AlphaVantageStocksApiSharp/Structs/AlphaVantageApiSymbols.cs

## Get TimeSeries

```cs
// This will return the last 100 datapoints, depending on the provided interval.
var series = await client.GetTimeSeriesAsync(AlphaVantageApiSymbols.MercedesBenzGroupAG, AlphaVantageApiTimeSeriesIntervals.Weekly);
```
