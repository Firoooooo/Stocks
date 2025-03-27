using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Import
{
    public class StockDataService
    {
        private string APIKey { get; set; }
        private string ConnectionString { get; set; }
        private JObject TimeSeriesData { get; set; }
        private HttpClient HTTPClient { get; set; }
        public List<StockPrice> StockPrices { get; set; }
            = new List<StockPrice>();


        public StockDataService(string API, string CON)
        {
            APIKey = API;
            ConnectionString = CON; // Evtl auslagern in eine andere Klasse
            HTTPClient = new HttpClient();
        }

        public async Task FetchAndStoreStockData(string _NASDAQS)
        {
            try
            {
                string aPIURL = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={_NASDAQS}&apikey={APIKey}";
                HttpClient hTTPClient = new HttpClient();  
                HttpResponseMessage hTTPResponse = await hTTPClient.GetAsync(aPIURL);

                if (!hTTPResponse.IsSuccessStatusCode)
                    throw new Exception($"API Anfrage für {_NASDAQS} ist fehlgeschlagen");

                string jSONResponse = await hTTPResponse.Content.ReadAsStringAsync();

                ParseStockData(jSONResponse);

            }
            catch (Exception EX)
            {
                Trace.TraceWarning(EX.Message);
            }
        }

        private void ParseStockData(string _jSONResponse)
        {
            JObject jSONParsed = JObject.Parse(_jSONResponse);
            TimeSeriesData = jSONParsed["Time Series (Daily)"] as JObject;

            if (TimeSeriesData != null)
            {
                StockPrices.AddRange(TimeSeriesData.Properties().Select(D => new StockPrice
                {
                    Symbol = jSONParsed["Meta Data"]["2. Symbol"].ToString(),
                    Date = DateTime.Parse(D.Name),
                    Open = decimal.Parse(D.Value["1. open"].ToString()),
                    High = decimal.Parse(D.Value["2. high"].ToString()),
                    Low = decimal.Parse(D.Value["3. low"].ToString()),
                    Close = decimal.Parse(D.Value["4. close"].ToString()),
                    Volume = long.Parse(D.Value["5. volume"].ToString())
                }));
            }
        }
    }
}
