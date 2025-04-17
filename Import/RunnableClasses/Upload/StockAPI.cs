namespace Import.RunnableClasses.Upload
{
    /// <summary>
    /// Liest Bezeichnungen aus einer Datei, ruft die zugehörigen Daten über eine API ab und speichert sie in einer Datenbank
    /// </summary>
    [Upload(1, "Erfasst aktuelle Aktien Informationen über die API und speichert die Daten in der Stock Tabelle")]
    public class StockAPI : RunnableClassBase
    {
        public StockDataService StockDataService { get; set; }


        /// <summary>
        /// reads files and then transmits the api responses to the database
        /// </summary>
        public override void Run()
        {
            StockDataService = new StockDataService();
            CheckForFiles();
        }

        /// <summary>
        /// checks whether the file exists or not
        /// </summary>
        public void CheckForFiles()
        {
            int sTOCKCounter = 1;

            FileReaderBase rESXFiles = GetRESXReader();
            rESXFiles.Stocks.ForEach(async F =>
            {
                await StockDataService.FetchAndStoreStockData(F, sTOCKCounter);
                sTOCKCounter++;
            });
            StockDataService.ImportDataToDB();
        }
    }
}
