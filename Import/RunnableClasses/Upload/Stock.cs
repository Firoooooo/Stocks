using Import.Context;

namespace Import.RunnableClasses.Upload
{
    /// <summary>
    /// Liest Bezeichnungen aus einer Datei, ruft die zugehörigen Daten über eine API ab und speichert sie in einer Datenbank
    /// </summary>
    [Upload(1, "Erfasst aktuelle Aktien Informationen über die API und speichert die Daten in der Stock Tabelle")]
    public class Stock : RunnableClassBase
    {
        public StockDataService StockDataService { get; set; }


        /// <summary>
        /// base call
        /// </summary>
        /// <param name="_cON">connection context</param>
        public Stock(Connections _cON) 
            : base(_cON)
        {
            CON = _cON;
        }

        /// <summary>
        /// reads files and then transmits the api responses to the database
        /// </summary>
        public override void Run()
        {
            StockDataService = new StockDataService(CON);
            CheckForFiles();
        }

        /// <summary>
        /// checks whether the file exists or not
        /// </summary>
        public void CheckForFiles()
        {
            FileReaderBase rESXFiles = GetRESXReader();
            rESXFiles.Stocks.ForEach(async F => await StockDataService.FetchAndStoreStockData(F));
            StockDataService.ImportDataToDB();
        }
    }
}
