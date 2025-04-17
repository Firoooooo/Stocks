using Import.Model;
using Import.Resources;
using Import.Singleton;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Import
{
    /// <summary>
    /// class for executing the api call and preparing the data
    /// </summary>
    public class StockDataService
    {
        private JObject TimeSeriesData { get; set; }
        private HttpClient HTTPClient { get; set; }
        public Dictionary<int, DTOStockPrice> StockPrices { get; set; }
            = new Dictionary<int, DTOStockPrice>();


        /// <summary>
        /// constructor that receives the context and creates an instance of the client class to execute the request
        /// </summary>
        public StockDataService()
        {
            HTTPClient = new HttpClient();
        }

        /// <summary>
        /// fetches stock data from the Alpha Vantage API and processes it
        /// </summary>
        /// <param name="_nASDAQS">the NASDAQ symbol of the stock for which data should be retrieved</param>
        /// <returns>Task</returns>
        public async Task FetchAndStoreStockData(string _nASDAQS, int _sTOCKCounter)
        { 
            try
            {
                string aPIURL = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={_nASDAQS}&apikey={Connections.xInstance.APIKEY}";
                HttpResponseMessage rESP = HTTPClient.GetAsync(aPIURL).Result;

                if (!rESP.IsSuccessStatusCode)
                    throw new Exception($"{Labels.APICallFailed} {_nASDAQS}");

                string jSON = await rESP.Content.ReadAsStringAsync();

                ParseStockData(jSON, _sTOCKCounter);
            }
            catch (Exception EX)
            {
                Trace.TraceWarning(EX.Message);
            }
        }

        /// <summary>
        /// parses the JSON response from the alpha vantage API and extracts stock price data
        /// </summary>
        /// <param name="_jSON">the JSON response string containing stock market data</param>
        private void ParseStockData(string _jSON, int _sTOCKCounter)
        {
            JObject jSONParsed = JObject.Parse(_jSON);
            TimeSeriesData = jSONParsed[Resources.Labels.TimeSeriesData] as JObject;

            if (TimeSeriesData != null && TimeSeriesData.Properties().Any())
            {
                StockPrices.Add(_sTOCKCounter, new DTOStockPrice
                {
                    Symbol = jSONParsed[Resources.Labels.MetaData][Resources.Labels.Symbol].ToString(),
                    Date = DateTime.Parse(TimeSeriesData.Properties().First().Name),
                    Open = decimal.Parse(TimeSeriesData.Properties().First().Value[Resources.Labels.Open].ToString()),
                    High = decimal.Parse(TimeSeriesData.Properties().First().Value[Resources.Labels.High].ToString()),
                    Low = decimal.Parse(TimeSeriesData.Properties().First().Value[Resources.Labels.Low].ToString()),
                    Close = decimal.Parse(TimeSeriesData.Properties().First().Value[Resources.Labels.Close].ToString()),
                    Volume = long.Parse(TimeSeriesData.Properties().First().Value[Resources.Labels.Volume].ToString())
                });
            }
        }

        /// <summary>
        /// inserts the stock data into the database
        /// </summary>
        public void ImportDataToDB()
        {
            try
            {
                using (MySqlConnection sQLCon = new MySqlConnection(Connections.xInstance.CONNECTIONSTRING))
                {
                    sQLCon.Open();
                    SQLInitializer.ExecuteQuery($"USE {Connections.xInstance.DATABASENAME};"
                        , sQLCon);

                    StockDataService.ImportIntoStock(sQLCon, StockPrices);
                }
            }
            catch (Exception EX)
            {
                Console.WriteLine($"{Import.Resources.Labels.DatabaseConFailed} {EX.Message}");
            }
        }

        /// <summary>
        /// inserts the stock data into the database
        /// </summary>
        /// <param name="_sQLConnection">sql connection</param>
        /// <param name="_rESXValues">res</param>
        public static void ImportIntoStock(MySqlConnection _sQLConnection, Dictionary<int, DTOStockPrice> _rESXValues)
        {
            string sQLQuery = @"INSERT INTO Stock (SYMBOL, DATE, OPEN, HIGH, LOW, CLOSE, VOLUME) VALUES (@SYMBOL, @DATE, @OPEN, @HIGH, @LOW, @CLOSE, @VOLUME) ON DUPLICATE KEY UPDATE OPEN = @OPEN, HIGH = @HIGH, LOW = @LOW, CLOSE = @CLOSE, VOLUME = @VOLUME, LASTUPDATED = CURRENT_TIMESTAMP;";

            using (MySqlTransaction sQLTransaction = _sQLConnection.BeginTransaction())
            using (MySqlCommand sQLCom = new MySqlCommand(sQLQuery, _sQLConnection, sQLTransaction))
            {
                try
                {
                    _rESXValues.ToList().ForEach(S =>
                    {
                        sQLCom.Parameters.Clear();

                        sQLCom.Parameters.AddWithValue("@SYMBOL", S.Value.Symbol);
                        sQLCom.Parameters.AddWithValue("@DATE", S.Value.Date);
                        sQLCom.Parameters.AddWithValue("@OPEN", S.Value.Open);
                        sQLCom.Parameters.AddWithValue("@HIGH", S.Value.High);
                        sQLCom.Parameters.AddWithValue("@LOW", S.Value.Low);
                        sQLCom.Parameters.AddWithValue("@CLOSE", S.Value.Close);
                        sQLCom.Parameters.AddWithValue("@VOLUME", S.Value.Volume);

                        sQLCom.ExecuteNonQuery();
                    });

                    sQLTransaction.Commit();
                }
                catch (Exception EX)
                {
                    sQLTransaction.Rollback();
                    Console.WriteLine($"{Import.Resources.Labels.DataImportFailed} {EX.Message}");
                }
            }
        }
    }
}
