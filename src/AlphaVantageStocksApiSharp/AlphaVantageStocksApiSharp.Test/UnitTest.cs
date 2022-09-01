using NUnit.Framework;
using AndreasReitberger.API;
using System;
using System.Threading.Tasks;
using AndreasReitberger.API.Structs;
using AndreasReitberger.API.Models.REST.Respones;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;
using AndreasReitberger.API.Enums;

namespace AlphaVantageStocksApiSharp.Test
{
    public class Tests
    {
        const string web = "https://www.alphavantage.co";
        const string api = "DNVJD58W1K6TFBFZ";
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ConnectionTest()
        {
            try
            {
                AlphaVantageClient client = new AlphaVantageClient.AlphaVantageConnectionBuilder()
                    .WithWebAddressAndApiKey(webAddress: web, apiKey: api)
                    .Build();
                Assert.IsNotNull(client);
                await client.CheckOnlineAsync();
                Assert.IsTrue(client.IsOnline);

                //List<AlphaVantageSymbolInfo> allSymbols = await client.GetUSSTocksAsync();
                //Assert.True(allSymbols?.Count > 0);

                var search = await client.SearchSymbolsAsync("E.ON");
                //var dow = await client.SearchSymbolsAsync("DOW");
                var series = await client.GetTimeSeriesAsync(AlphaVantageApiSymbols.MercedesBenzGroupAG, AlphaVantageApiTimeSeriesIntervals.Weekly);

                for (int i = 65; i < 65 + 26; i++)
                {
                    char first = (char)i;
                    for (int j = 97; j < 97 + 26; j++)
                    {
                        char second = (char)j;
                        string keword = $"{first}{second}";
                        var list = await client.SearchSymbolsAsync(keword);

                        List<string> searchResult = list?.BestMatches?.Select(match => match.Symbol).ToList();
                        Debug.WriteLine($"{keword}: {string.Join(";", searchResult)}");
                        // The api seems to have a cooldown time
                        await Task.Delay(100);
                    }
                }

                var symbols = await client.SearchSymbolsAsync("Merc");
                var result = await client.GetIntradayAsync(AlphaVantageApiSymbols.IBM);
                Assert.IsNotNull(result);
                //Assert.Pass();
            }
            catch(Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }
    }
}