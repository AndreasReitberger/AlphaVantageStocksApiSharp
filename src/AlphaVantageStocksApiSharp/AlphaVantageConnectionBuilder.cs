using System;
using System.Collections.Generic;
using System.Text;

namespace AndreasReitberger.API
{
    public partial class AlphaVantageClient
    {
        public class AlphaVantageConnectionBuilder
        {
            #region Instance
            readonly AlphaVantageClient _client = new();
            #endregion

            #region Methods

            public AlphaVantageClient Build()
            {
                return _client;
            }

            public AlphaVantageConnectionBuilder WithWebAddress(string webAddress)
            {
                _client.Address = webAddress;
                return this;
            }

            public AlphaVantageConnectionBuilder WithApiKey(string apiKey)
            {
                _client.ApiKey = apiKey;
                return this;
            }

            public AlphaVantageConnectionBuilder WithWebAddressAndApiKey(string webAddress, string apiKey)
            {
                _client.Address = webAddress;
                _client.ApiKey = apiKey;
                return this;
            }

            #endregion
        }
    }
}
