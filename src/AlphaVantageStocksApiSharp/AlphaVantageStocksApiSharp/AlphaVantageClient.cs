using AndreasReitberger.API.Enums;
using AndreasReitberger.API.Models.REST;
using AndreasReitberger.API.Models.REST.Events;
using AndreasReitberger.API.Models.REST.Respones;
using AndreasReitberger.API.Structs;
using AndreasReitberger.Core.Interfaces;
using AndreasReitberger.Core.Utilities;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AndreasReitberger.API
{
    // Documentation: https://www.alphavantage.co/documentation/
    public partial class AlphaVantageClient : BaseModel
    {
        #region Instance
        static AlphaVantageClient _instance = null;
        static readonly object Lock = new();
        public static AlphaVantageClient Instance
        {
            get
            {
                lock (Lock)
                {
                    if (_instance == null)
                        _instance = new AlphaVantageClient();
                }
                return _instance;
            }
            set
            {
                if (_instance == value) return;
                lock (Lock)
                {
                    _instance = value;
                }
            }
        }

        #endregion

        #region Variables
        RestClient restClient;
        HttpClient httpClient;
        int _retries = 0;
        #endregion

        #region Properties
        [JsonProperty(nameof(ApiKey))]
        string apiKey = "";
        [JsonIgnore]
        public string ApiKey
        {
            get => apiKey;
            set
            {
                if (apiKey == value)
                    return;
                apiKey = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(Address))]
        string address = "https://www.alphavantage.co";
        [JsonIgnore]
        public string Address
        {
            get => address;
            set
            {
                if (address == value)
                    return;
                address = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(DefaultTimeout))]
        bool isOnline = false;
        [JsonIgnore]
        public bool IsOnline
        {
            get => isOnline;
            set
            {
                if (isOnline != value)
                {
                    isOnline = value;
                    OnPropertyChanged();
                }
            }
        }

        [JsonIgnore, XmlIgnore]
        bool _isConnecting = false;
        [JsonIgnore, XmlIgnore]
        public bool IsConnecting
        {
            get => _isConnecting;
            set
            {
                if (_isConnecting == value) return;
                _isConnecting = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(DefaultTimeout))]
        int defaultTimeout = 10000;
        [JsonIgnore]
        public int DefaultTimeout
        {
            get => defaultTimeout;
            set
            {
                if (defaultTimeout != value)
                {
                    defaultTimeout = value;
                    OnPropertyChanged();
                }
            }
        }

        [JsonProperty(nameof(RetriesWhenOffline))]
        [XmlAttribute(nameof(RetriesWhenOffline))]
        int _retriesWhenOffline = 2;
        [JsonIgnore, XmlIgnore]
        public int RetriesWhenOffline
        {
            get => _retriesWhenOffline;
            set
            {
                if (_retriesWhenOffline == value) return;
                _retriesWhenOffline = value;
                OnPropertyChanged();
            }
        }

        #region Proxy
        [JsonProperty(nameof(EnableProxy))]
        [XmlAttribute(nameof(EnableProxy))]
        bool _enableProxy = false;
        [JsonIgnore, XmlIgnore]
        public bool EnableProxy
        {
            get => _enableProxy;
            set
            {
                if (_enableProxy == value) return;
                _enableProxy = value;
                OnPropertyChanged();
                UpdateRestClientInstance();
            }
        }

        [JsonProperty(nameof(ProxyUseDefaultCredentials))]
        [XmlAttribute(nameof(ProxyUseDefaultCredentials))]
        bool _proxyUseDefaultCredentials = true;
        [JsonIgnore, XmlIgnore]
        public bool ProxyUseDefaultCredentials
        {
            get => _proxyUseDefaultCredentials;
            set
            {
                if (_proxyUseDefaultCredentials == value) return;
                _proxyUseDefaultCredentials = value;
                OnPropertyChanged();
                UpdateRestClientInstance();
            }
        }

        [JsonProperty(nameof(SecureProxyConnection))]
        [XmlAttribute(nameof(SecureProxyConnection))]
        bool _secureProxyConnection = true;
        [JsonIgnore, XmlIgnore]
        public bool SecureProxyConnection
        {
            get => _secureProxyConnection;
            private set
            {
                if (_secureProxyConnection == value) return;
                _secureProxyConnection = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ProxyAddress))]
        [XmlAttribute(nameof(ProxyAddress))]
        string _proxyAddress = string.Empty;
        [JsonIgnore, XmlIgnore]
        public string ProxyAddress
        {
            get => _proxyAddress;
            private set
            {
                if (_proxyAddress == value) return;
                _proxyAddress = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ProxyPort))]
        [XmlAttribute(nameof(ProxyPort))]
        int _proxyPort = 443;
        [JsonIgnore, XmlIgnore]
        public int ProxyPort
        {
            get => _proxyPort;
            private set
            {
                if (_proxyPort == value) return;
                _proxyPort = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ProxyUser))]
        [XmlAttribute(nameof(ProxyUser))]
        string _proxyUser = string.Empty;
        [JsonIgnore, XmlIgnore]
        public string ProxyUser
        {
            get => _proxyUser;
            private set
            {
                if (_proxyUser == value) return;
                _proxyUser = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ProxyPassword))]
        [XmlAttribute(nameof(ProxyPassword))]
        SecureString _proxyPassword;
        [JsonIgnore, XmlIgnore]
        public SecureString ProxyPassword
        {
            get => _proxyPassword;
            private set
            {
                if (_proxyPassword == value) return;
                _proxyPassword = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #endregion

        #region Constructor
        public AlphaVantageClient() { }

        public AlphaVantageClient(string apiKey)
        {
            ApiKey = apiKey;
        }

        public AlphaVantageClient(string webAddress, string apiKey)
        {
            Address = webAddress;
            ApiKey = apiKey;
        }
        #endregion

        #region EventHandlers
        public event EventHandler Error;
        protected virtual void OnError()
        {
            Error?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnError(ErrorEventArgs e)
        {
            Error?.Invoke(this, e);
        }
        protected virtual void OnError(UnhandledExceptionEventArgs e)
        {
            Error?.Invoke(this, e);
        }
        protected virtual void OnError(AlphaVantageJsonConvertEventArgs e)
        {
            Error?.Invoke(this, e);
        }
        public event EventHandler<AlphaVantageRestEventArgs> RestApiError;
        protected virtual void OnRestApiError(AlphaVantageRestEventArgs e)
        {
            RestApiError?.Invoke(this, e);
        }

        public event EventHandler<AlphaVantageJsonConvertEventArgs> RestJsonConvertError;
        protected virtual void OnRestJsonConvertError(AlphaVantageJsonConvertEventArgs e)
        {
            RestJsonConvertError?.Invoke(this, e);
        }
        #endregion

        #region Methods

        #region Proxy
        Uri GetProxyUri()
        {
            return ProxyAddress.StartsWith("http://") || ProxyAddress.StartsWith("https://") ? new Uri($"{ProxyAddress}:{ProxyPort}") : new Uri($"{(SecureProxyConnection ? "https" : "http")}://{ProxyAddress}:{ProxyPort}");
        }

        WebProxy GetCurrentProxy()
        {
            WebProxy proxy = new()
            {
                Address = GetProxyUri(),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = ProxyUseDefaultCredentials,
            };
            if (ProxyUseDefaultCredentials && !string.IsNullOrEmpty(ProxyUser))
            {
                proxy.Credentials = new NetworkCredential(ProxyUser, ProxyPassword);
            }
            else
            {
                proxy.UseDefaultCredentials = ProxyUseDefaultCredentials;
            }
            return proxy;
        }
        void UpdateRestClientInstance()
        {
            if (string.IsNullOrEmpty(Address))
            {
                return;
            }
            if (EnableProxy && !string.IsNullOrEmpty(ProxyAddress))
            {
                RestClientOptions options = new(Address)
                {
                    ThrowOnAnyError = true,
                    Timeout = 10000,
                };
                HttpClientHandler httpHandler = new()
                {
                    UseProxy = true,
                    Proxy = GetCurrentProxy(),
                    AllowAutoRedirect = true,
                };

                httpClient = new(handler: httpHandler, disposeHandler: true);
                restClient = new(httpClient: httpClient, options: options);
            }
            else
            {
                httpClient = null;
                restClient = new(baseUrl: Address);
            }
        }
        #endregion

        #region OnlineCheck

        public async Task CheckOnlineAsync(int timeout = 10000)
        {
            CancellationTokenSource cts = new(timeout);
            await CheckOnlineAsync(cts).ConfigureAwait(false);
        }

        public async Task CheckOnlineAsync(CancellationTokenSource cts)
        {
            if (IsConnecting) return; // Avoid multiple calls
            IsConnecting = true;
            bool isReachable = false;
            try
            {
                string uriString = Address;
                try
                {
                    // Send a blank api request in order to check if the server is reachable
                    AlphaVantageApiRequestRespone respone = await SendOnlineCheckRestApiRequestAsync(
                       function: AlphaVantageApiFunctions.TIME_SERIES_INTRADAY,
                       //symbol: AlphaVantageApiSymbols.IBM,
                       additionalParameters: new Dictionary<string, string>() { { "symbol", AlphaVantageApiSymbols.IBM }, { "interval", "1min" } },
                       cts: cts)
                    .ConfigureAwait(false);

                    isReachable = respone?.IsOnline == true;
                }
                catch (InvalidOperationException iexc)
                {
                    OnError(new UnhandledExceptionEventArgs(iexc, false));
                }
                catch (HttpRequestException rexc)
                {
                    OnError(new UnhandledExceptionEventArgs(rexc, false));
                }
                catch (TaskCanceledException)
                {
                    // Throws an exception on timeout, not actually an error
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            IsConnecting = false;
            // Avoid offline message for short connection loss
            if (!IsOnline || isReachable || _retries > RetriesWhenOffline)
            {
                // Do not check if the previous state was already offline
                _retries = 0;
                IsOnline = isReachable;
            }
            else
            {
                // Retry with shorter timeout to see if the connection loss is real
                _retries++;
                cts = new(3500);
                await CheckOnlineAsync(cts).ConfigureAwait(false);
            }
        }

        #endregion

        #region REST
        async Task<AlphaVantageApiRequestRespone> SendRestApiRequestAsync(
            AlphaVantageApiFunctions function,
            //AlphaVantageApiSymbols symbol = AlphaVantageApiSymbols.None,
            Dictionary<string, string> additionalParameters = null,
            object jsonData = null,
            Method method = Method.Get,
            CancellationTokenSource cts = default,
            string requestTargetUri = "")
        {
            AlphaVantageApiRequestRespone apiRsponeResult = new() { IsOnline = IsOnline };
            if (!IsOnline) return apiRsponeResult;
            try
            {
                if (cts == default)
                {
                    cts = new(DefaultTimeout);
                }
                if (restClient == null)
                {
                    UpdateRestClientInstance();
                }
                RestRequest request = new(
                    string.IsNullOrEmpty(requestTargetUri) ?
                    "query":
                    requestTargetUri)
                {
                    RequestFormat = DataFormat.Json,
                    Method = method
                };

                request.AddParameter("function", function.ToString(), ParameterType.QueryString);
                /*
                if(symbol != AlphaVantageApiSymbols.None)
                {
                    request.AddParameter("symbol", symbol.ToString(), ParameterType.QueryString);
                }
                */
                if (additionalParameters?.Count > 0)
                {
                    foreach(KeyValuePair<string, string> paramter in additionalParameters)
                    {
                        request.AddParameter(paramter.Key, paramter.Value, ParameterType.QueryString);
                    }
                }

                string jsonDataString = "";
                if (jsonData is string jsonString)
                {
                    jsonDataString = jsonString;
                }
                else if (jsonData is object jsonObject)
                {
                    jsonDataString = JsonConvert.SerializeObject(jsonObject);
                }
                if(!string.IsNullOrEmpty(jsonDataString))
                {
                    _ = request.AddParameter("data", jsonDataString, ParameterType.QueryString);
                }

                if (!string.IsNullOrEmpty(ApiKey))
                {
                    request.AddParameter("apikey", ApiKey, ParameterType.QueryString);
                }
                else 
                {
                    request.AddParameter("apikey", "demo", ParameterType.QueryString);
                }

                Uri fullUri = restClient.BuildUri(request);
                try
                {
                    RestResponse respone = await restClient.ExecuteAsync(request, cts.Token).ConfigureAwait(false);
                    apiRsponeResult = ValidateRespone(respone, fullUri);
                }
                catch (TaskCanceledException texp)
                {
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(texp, false));
                    }
                    // Throws exception on timeout, not actually an error but indicates if the server is reachable.
                }
                catch (HttpRequestException hexp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(hexp, false));
                    }
                }
                catch (TimeoutException toexp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(toexp, false));
                    }
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            return apiRsponeResult;
        }

        async Task<AlphaVantageApiRequestRespone> SendOnlineCheckRestApiRequestAsync(
            AlphaVantageApiFunctions function,
            //AlphaVantageApiSymbols symbol,
            Dictionary<string, string> additionalParameters = null,
            Method method = Method.Get,
            CancellationTokenSource cts = default,
            string requestTargetUri = ""
            )
        {
            AlphaVantageApiRequestRespone apiRsponeResult = new() { IsOnline = false };
            try
            {
                if (restClient == null)
                {
                    UpdateRestClientInstance();
                }
                RestRequest request = new(
                    string.IsNullOrEmpty(requestTargetUri) ?
                    "query" :
                    requestTargetUri)
                {
                    RequestFormat = DataFormat.Json,
                    Method = method
                };
                request.AddParameter("function", function.ToString(), ParameterType.QueryString);
                //request.AddParameter("symbol", symbol.ToString(), ParameterType.QueryString);

                if (additionalParameters?.Count > 0)
                {
                    foreach (KeyValuePair<string, string> paramter in additionalParameters)
                    {
                        request.AddParameter(paramter.Key, paramter.Value, ParameterType.QueryString);
                    }
                }

                if (!string.IsNullOrEmpty(ApiKey))
                {
                    request.AddParameter("apikey", ApiKey, ParameterType.QueryString);
                }
                else
                {
                    request.AddParameter("apikey", "demo", ParameterType.QueryString);
                }

                Uri fullUri = restClient.BuildUri(request);
                try
                {
                    RestResponse respone = await restClient.ExecuteAsync(request, cts.Token).ConfigureAwait(false);
                    apiRsponeResult = ValidateRespone(respone, fullUri);
                }
                catch (TaskCanceledException)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                }
                catch (HttpRequestException)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                }
                catch (TimeoutException)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                }

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            return apiRsponeResult;
        }

        AlphaVantageApiRequestRespone ValidateRespone(RestResponse respone, Uri targetUri)
        {
            AlphaVantageApiRequestRespone apiRsponeResult = new() { IsOnline = IsOnline };
            try
            {
                if ((
                    respone.StatusCode == HttpStatusCode.OK || respone.StatusCode == HttpStatusCode.NoContent) &&
                    respone.ResponseStatus == ResponseStatus.Completed)
                {
                    apiRsponeResult.IsOnline = true;
                    //AuthenticationFailed = false;
                    apiRsponeResult.Result = respone.Content;
                    apiRsponeResult.Succeeded = true;
                    apiRsponeResult.EventArgs = new AlphaVantageRestEventArgs()
                    {
                        Status = respone.ResponseStatus.ToString(),
                        Exception = respone.ErrorException,
                        Message = respone.ErrorMessage,
                        Uri = targetUri,
                    };
                }
                else if (respone.StatusCode == HttpStatusCode.NonAuthoritativeInformation
                    || respone.StatusCode == HttpStatusCode.Forbidden
                    || respone.StatusCode == HttpStatusCode.Unauthorized
                    )
                {
                    apiRsponeResult.IsOnline = true;
                    apiRsponeResult.HasAuthenticationError = true;
                    apiRsponeResult.EventArgs = new AlphaVantageRestEventArgs()
                    {
                        Status = respone.ResponseStatus.ToString(),
                        Exception = respone.ErrorException,
                        Message = respone.ErrorMessage,
                        Uri = targetUri,
                    };
                }
                else if (respone.StatusCode == HttpStatusCode.Conflict)
                {
                    apiRsponeResult.IsOnline = true;
                    apiRsponeResult.HasAuthenticationError = false;
                    apiRsponeResult.EventArgs = new AlphaVantageRestEventArgs()
                    {
                        Status = respone.ResponseStatus.ToString(),
                        Exception = respone.ErrorException,
                        Message = respone.ErrorMessage,
                        Uri = targetUri,
                    };
                }
                else
                {
                    OnRestApiError(new AlphaVantageRestEventArgs()
                    {
                        Status = respone.ResponseStatus.ToString(),
                        Exception = respone.ErrorException,
                        Message = respone.ErrorMessage,
                        Uri = targetUri,
                    });
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            return apiRsponeResult;
        }
        #endregion

        #region Public

        public async Task<List<AlphaVantageSymbolInfo>> GetUSSTocksAsync()
        {
            List<AlphaVantageSymbolInfo> returnValue = new();
            AlphaVantageApiRequestRespone result = new();
            try
            {
                // Always seems to be a CSV
                Dictionary<string, string> parameters = new()
                {
                    { "datatype", AlphaVantageApiDataTypes.Json.ToString().ToLower()},
                };

                result = await SendRestApiRequestAsync(
                   function: AlphaVantageApiFunctions.LISTING_STATUS,
                   additionalParameters: parameters
                   )
                    .ConfigureAwait(false);

                var lines = result.Result.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .Skip(1) // Skip header line
                    .ToList();
                for(int i = 0; lines.Count > 0; i++)
                {
                    var entry = lines[i].Split(',');
                    try
                    {
                        AlphaVantageSymbolInfo info = new()
                        {
                            Symbol = entry[0],
                            Name = entry[1],
                            Exchange = entry[2],
                            AssetType = entry[3],
                            Status = entry[6],
                        };
                        if(!string.IsNullOrEmpty(entry[4]) && entry[4] != "null")
                        {
                            info.IpoDate = DateTime.Parse(entry[4]);
                        }
                        if(!string.IsNullOrEmpty(entry[5]) && entry[5] != "null")
                        {
                            info.DelistingDate = DateTime.Parse(entry[5]);
                        }
                        returnValue.Add(info);
                    }
                    catch(Exception exc)
                    {
                        OnError(new UnhandledExceptionEventArgs(exc, false));
                        continue;
                    }
                }
                return returnValue;
            }
            catch (JsonException jecx)
            {
                OnError(new AlphaVantageJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(IsOnline),
                    Message = jecx.Message,
                });
                return returnValue;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return returnValue;
            }
        }

        public async Task<AlphaVantageSymbolSearchRespone> SearchSymbolsAsync(string keywords)
        {
            AlphaVantageApiRequestRespone result = new();
            try
            {
                Dictionary<string, string> parameters = new()
                {
                    { "datatype", AlphaVantageApiDataTypes.Json.ToString().ToLower()},
                    { "keywords", keywords}
                };

                result = await SendRestApiRequestAsync(
                   function: AlphaVantageApiFunctions.SYMBOL_SEARCH,
                   additionalParameters: parameters
                   )
                    .ConfigureAwait(false);
                //result.Result = result.Result.Replace($" ({parameters["interval"]})", string.Empty);
                AlphaVantageSymbolSearchRespone info = JsonConvert.DeserializeObject<AlphaVantageSymbolSearchRespone>(result.Result);
                return info;
            }
            catch (JsonException jecx)
            {
                OnError(new AlphaVantageJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(IsOnline),
                    Message = jecx.Message,
                });
                return new AlphaVantageSymbolSearchRespone();
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new AlphaVantageSymbolSearchRespone();
            }
        }
        
        public async Task<AlphaVantageTimeSeriesRespone> GetIntradayAsync(string symbol, AlphaVantageApiIntervals interval = AlphaVantageApiIntervals.Min1)
        {
            AlphaVantageApiRequestRespone result = new();
            try
            {
                Dictionary<string, string> parameters = new()
                {
                    { "symbol", symbol },
                    { "datatype", AlphaVantageApiDataTypes.Json.ToString().ToLower()}
                };
                string key = "interval";
                switch (interval)
                {
                    case AlphaVantageApiIntervals.Min1:
                        parameters.Add(key, "1min");
                        break;
                    case AlphaVantageApiIntervals.Min5:
                        parameters.Add(key, "5min");
                        break;
                    case AlphaVantageApiIntervals.Min15:
                        parameters.Add(key, "15min");
                        break;
                    case AlphaVantageApiIntervals.Min30:
                        parameters.Add(key, "30min");
                        break;
                    case AlphaVantageApiIntervals.Min60:
                        parameters.Add(key, "60min");
                        break;
                    default:
                        break;
                }

                result = await SendRestApiRequestAsync(
                   function: AlphaVantageApiFunctions.TIME_SERIES_INTRADAY,
                   additionalParameters: parameters
                   )
                    .ConfigureAwait(false);
                result.Result = result.Result.Replace($" ({parameters["interval"]})", string.Empty);
                AlphaVantageTimeSeriesRespone info = JsonConvert.DeserializeObject<AlphaVantageTimeSeriesRespone>(result.Result);
                return info;
            }
            catch (JsonException jecx)
            {
                OnError(new AlphaVantageJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(IsOnline),
                    Message = jecx.Message,
                });
                return new AlphaVantageTimeSeriesRespone();
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new AlphaVantageTimeSeriesRespone();
            }
        }

        /*
        public async Task<AlphaVantageIntradayRespone> GetIntradayAsync(AlphaVantageApiSymbols symbol, AlphaVantageApiIntervals interval = AlphaVantageApiIntervals.Min1)
        {
            return await GetIntradayAsync(symbol, interval).ConfigureAwait(false);
        }
        */

        public async Task<AlphaVantageTimeSeriesRespone> GetTimeSeriesAsync(string symbol, AlphaVantageApiTimeSeriesIntervals interval = AlphaVantageApiTimeSeriesIntervals.Daily)
        {
            AlphaVantageApiRequestRespone result = new();
            try
            {
                Dictionary<string, string> parameters = new()
                {
                    { "symbol", symbol },
                    { "datatype", AlphaVantageApiDataTypes.Json.ToString().ToLower() }
                };
                AlphaVantageApiFunctions function = AlphaVantageApiFunctions.TIME_SERIES_DAILY;
                function = interval switch
                {
                    AlphaVantageApiTimeSeriesIntervals.DailyAdjusted => AlphaVantageApiFunctions.TIME_SERIES_DAILY_ADJUSTED,
                    AlphaVantageApiTimeSeriesIntervals.Weekly => AlphaVantageApiFunctions.TIME_SERIES_WEEKLY,
                    AlphaVantageApiTimeSeriesIntervals.WeeklyAdjusted => AlphaVantageApiFunctions.TIME_SERIES_WEEKLY_ADJUSTED,
                    AlphaVantageApiTimeSeriesIntervals.Monthly => AlphaVantageApiFunctions.TIME_SERIES_MONTHLY,
                    AlphaVantageApiTimeSeriesIntervals.MonthlyAdjusted => AlphaVantageApiFunctions.TIME_SERIES_MONTHLY_ADJUSTED,
                    _ => AlphaVantageApiFunctions.TIME_SERIES_DAILY,
                };
                result = await SendRestApiRequestAsync(
                   function: function,
                   additionalParameters: parameters
                   )
                    .ConfigureAwait(false);
                result.Result = result.Result
                    .Replace($"Daily ", string.Empty)
                    .Replace($"Weekly ", string.Empty)
                    .Replace($"Monthly ", string.Empty);
                AlphaVantageTimeSeriesRespone info = JsonConvert.DeserializeObject<AlphaVantageTimeSeriesRespone>(result.Result);
                return info;
            }
            catch (JsonException jecx)
            {
                OnError(new AlphaVantageJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(IsOnline),
                    Message = jecx.Message,
                });
                return new AlphaVantageTimeSeriesRespone();
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new AlphaVantageTimeSeriesRespone();
            }
        }

        #endregion

        #endregion
    }
}
