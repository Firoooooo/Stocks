using Import.Context;
using Import.Resources;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Import
{
    /// <summary>
    /// class for executing the api call and preparing the data
    /// </summary>
    public class StockDataService
    {
        private Connections CON { get; set; }
        private JObject TimeSeriesData { get; set; }
        private HttpClient HTTPClient { get; set; }
        public List<StockPrice> StockPrices { get; set; }
            = new List<StockPrice>();


        /// <summary>
        /// constructor that receives the context and creates an instance of the client class to execute the request
        /// </summary>
        /// <param name="_cON">context class</param>
        public StockDataService(Connections _cON)
        {
            CON = _cON;
            HTTPClient = new HttpClient();
        }

        /// <summary>
        /// fetches stock data from the Alpha Vantage API and processes it
        /// </summary>
        /// <param name="_nASDAQS">the NASDAQ symbol of the stock for which data should be retrieved</param>
        /// <returns>Task</returns>
        public async Task FetchAndStoreStockData(string _nASDAQS)
        { 
            try
            {
                string aPIURL = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={_nASDAQS}&apikey={CON.APIKEY}";
                HttpResponseMessage rESP = HTTPClient.GetAsync(aPIURL).Result;

                if (!rESP.IsSuccessStatusCode)
                    throw new Exception($"{Labels.APICallFailed} {_nASDAQS}");

                string jSON = await rESP.Content.ReadAsStringAsync();

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
        /// <param name="_jSON">the JSON response string containing stock market data</param>
        private void ParseStockData(string _jSON)
        {
            JObject jSONParsed = JObject.Parse(_jSON);
            TimeSeriesData = jSONParsed[Resources.Labels.TimeSeriesData] as JObject;

            if (TimeSeriesData != null && TimeSeriesData.Properties().Any())
            {
                StockPrices.Add(new StockPrice
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
                using (MySqlConnection sQLCon = new MySqlConnection(CON.CONNECTIONSTRING))
                {
                    sQLCon.Open();
                    SQLInitializer.ExecuteQuery($"USE {CON.DATABASENAME};", sQLCon);

                    string sQLQuery = @"INSERT INTO Stock(SYMBOL, DATE, OPEN, HIGH, LOW, CLOSE, VOLUME) VALUES (@SYMBOL, @DATE, @OPEN, @HIGH, @LOW, @CLOSE, @VOLUME) ON DUPLICATE KEY UPDATE OPEN = @OPEN, HIGH = @HIGH, LOW = @LOW, CLOSE = @CLOSE, VOLUME = @VOLUME, LASTUPDATED = CURRENT_TIMESTAMP;";

                    using (MySqlTransaction sQLTransaction = sQLCon.BeginTransaction())
                    using (MySqlCommand sQLCom = new MySqlCommand(sQLQuery, sQLCon, sQLTransaction))
                    {
                        try
                        {
                            StockPrices.ForEach(P =>
                            {
                                sQLCom.Parameters.Clear();

                                sQLCom.Parameters.AddWithValue("@SYMBOL", P.Symbol);
                                sQLCom.Parameters.AddWithValue("@DATE", P.Date);
                                sQLCom.Parameters.AddWithValue("@OPEN", P.Open);
                                sQLCom.Parameters.AddWithValue("@HIGH", P.High);
                                sQLCom.Parameters.AddWithValue("@LOW", P.Low);
                                sQLCom.Parameters.AddWithValue("@CLOSE", P.Close);
                                sQLCom.Parameters.AddWithValue("@VOLUME", P.Volume);

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
            catch (Exception EX)
            {
                Console.WriteLine($"{Import.Resources.Labels.DatabaseConFailed} {EX.Message}");
            }
        }

    }
}
