using Import.Model;

namespace Import.RunnableClasses.Upload
{
    /// <summary>
    /// Liest Bezeichnungen aus einer Datei, ruft die zugehörigen Daten über eine API ab und speichert sie in einer Datenbank
    /// </summary>
    [Upload(2, "Importiert Stockdaten und speichert diese in der Stock Tabelle")]
    public class Stock : RunnableClassBase
    {
        public static Dictionary<int, DTOStockPrice> StockPriceMap { get; set; } 

        /// <summary>
        /// reads files and then transmits the api responses to the database
        /// </summary>
        public override void Run()
        {
            CheckForFiles();
        }

        /// <summary>
        /// checks whether the file exists or not
        /// </summary>
        public void CheckForFiles()
        {
            FileReaderBase rESXFiles = GetRESXReader();
            CreateStockPriceMap(rESXFiles.Stocks);
            SQLInitializer sQLInitializer = new SQLInitializer();
            sQLInitializer.InsertInStock(rESXFiles.RESXFile, rESXFiles.GetFiles());
        }

        /// <summary>
        /// creates a dictionary of stock prices
        /// </summary>
        /// <param name="_rESX">resx file</param>
        private void CreateStockPriceMap(List<string> _rESX)
        {
            StockPriceMap  = new Dictionary<int, DTOStockPrice>();
            int rESXC = 1;

            _rESX.ForEach(S =>
            {
                string[] rESXCSplit = S.Split(new[] { ' ', '\t' });
                StockPriceMap.Add(rESXC, new DTOStockPrice()
                {
                    Symbol = rESXCSplit[0].ToString(), Date = DateTime.Parse(rESXCSplit[1]), Open = decimal.Parse(rESXCSplit[2]), High = decimal.Parse(rESXCSplit[3]), Low = decimal.Parse(rESXCSplit[4]), Close = decimal.Parse(rESXCSplit[5]), Volume = long.Parse(rESXCSplit[6])
                });
                rESXC++;
            });
        }
    }
}
