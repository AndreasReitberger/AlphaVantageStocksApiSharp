using NUnit.Framework;
using AndreasReitberger.API;
using System;
using System.Threading.Tasks;
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