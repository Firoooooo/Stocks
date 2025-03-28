using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Import.Resources;
using System.Net.Http;

namespace Import
{
    /// <summary>
    /// class for executing the api call and preparing the data
    /// </summary>
    public class StockDataService
    {
        private string APIKey { get; set; }
        private string ConnectionString { get; set; }
        private JObject TimeSeriesData { get; set; }
        private HttpClient HTTPClient { get; set; }
        public List<StockPrice> StockPrices { get; set; }
            = new List<StockPrice>();


        /// <summary>
        /// overwrite the cosntrucutor with the api key and connection string
        /// </summary>
        /// <param name="API">api key</param>
        /// <param name="CON">connection string</param>
        public StockDataService(string API, string CON)
        {
            APIKey = API;
            ConnectionString = CON; // Evtl auslagern in eine andere Klasse
            HTTPClient = new HttpClient();
        }

        /// <summary>
        /// fetches stock data from the Alpha Vantage API and processes it
        /// </summary>
        /// <param name="_NASDAQS">the NASDAQ symbol of the stock for which data should be retrieved</param>
        /// <returns>Task</returns>
        public async Task FetchAndStoreStockData(string _NASDAQS)
        { 
            try
            {
                string aPIURL = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={_NASDAQS}&apikey={APIKey}";
                HttpResponseMessage hTTPResponse = HTTPClient.GetAsync(aPIURL).Result;

                if (!hTTPResponse.IsSuccessStatusCode)
                    throw new Exception($"API CALL ist fehlgeschlagen {_NASDAQS} ");

                string jSON = await hTTPResponse.Content.ReadAsStringAsync();

                ParseStockData(jSON);
            }
            catch (Exception EX)
            {
                Trace.TraceWarning(EX.Message);
            }
        }

        /// <summary>
        /// parses the JSON response from the alpha vantage API and extracts stock price data
        /// </summary>
        /// <param name="_jSONResponse">the JSON response string containing stock market data</param>
        private void ParseStockData(string _jSONResponse)
        {
            JObject jSONParsed = JObject.Parse(_jSONResponse);
            TimeSeriesData = jSONParsed[Resources.Labels.TimeSeriesData] as JObject;

            if (TimeSeriesData != null)
            {
                StockPrices.AddRange(TimeSeriesData.Properties().Select(D => new StockPrice
                {
                    Symbol = jSONParsed[Resources.Labels.MetaData][Resources.Labels.Symbol].ToString(),
                    Date = DateTime.Parse(D.Name),
                    Open = decimal.Parse(D.Value[Resources.Labels.Open].ToString()),
                    High = decimal.Parse(D.Value[Resources.Labels.High].ToString()),
                    Low = decimal.Parse(D.Value[Resources.Labels.Low].ToString()),
                    Close = decimal.Parse(D.Value[Resources.Labels.Close].ToString()),
                    Volume = long.Parse(D.Value[Resources.Labels.Volume].ToString())
                }));
            }
        }
    }
}
