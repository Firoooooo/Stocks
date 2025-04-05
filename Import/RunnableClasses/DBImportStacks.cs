using Import.Context;
using Import.Factories;
using Import.Resources;

namespace Import.RunnableClasses
{
    /// <summary>
    /// Liest Bezeichnungen aus einer Datei, ruft die zugehörigen Daten über eine API ab und speichert sie in einer Datenbank
    /// </summary>
    [RunnableClassAttribute(2, "Liest Bezeichnungen aus einer Datei, ruft die zugehörigen Daten über eine API ab und speichert sie in einer Datenbank")]
    public class DBImportStacks : RunnableClassBase
    {
        public StockDataService StockDataService { get; set; }

        /// <summary>
        /// base call
        /// </summary>
        /// <param name="_cON">connection context</param>
        public DBImportStacks(Connections _cON) 
            : base(_cON)
        {
            CON = _cON;
        }

        /// <summary>
        /// reads files and then transmits the api responses to the database
        /// </summary>
        public override void Run()
        {
            StockDataService = new StockDataService(CON.APIKEY, CON.CONNECTIONSTRING);
            ValidateFileToProcess();
        }

        /// <summary>
        /// checks whether the file exists or not
        /// </summary>
        public void ValidateFileToProcess()
        {
            string rESXFilePath = string.Empty;
            Console.WriteLine(Labels.EnterFile);

            while (true)
            {
                rESXFilePath = Console.ReadLine();
                if (File.Exists(rESXFilePath))
                    break;
                else
                    Console.WriteLine(Labels.FileDoesntExists);
            }

            FileReaderBase rESXFiles = FileReaderFactory.GetReader(rESXFilePath);
            rESXFiles.GetFiles().ForEach(async F => await StockDataService.FetchAndStoreStockData(F));
        }
    }
}
