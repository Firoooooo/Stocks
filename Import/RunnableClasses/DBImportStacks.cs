using Import.Context;
using Import.Factories;

namespace Import.RunnableClasses
{
    /// <summary>
    /// Liest Bezeichnungen aus einer Datei, ruft die zugehörigen Daten über eine API ab und speichert sie in einer Datenbank
    /// </summary>
    [RunnableClassAttribute(1, "Liest Bezeichnungen aus einer Datei, ruft die zugehörigen Daten über eine API ab und speichert sie in einer Datenbank")]
    public class DBImportStacks : RunnableClassBase
    {
        /// <summary>
        /// base call
        /// </summary>
        /// <param name="_cON">connection context</param>
        public DBImportStacks(Connections _cON) 
            : base(_cON)
        {
            CON = _cON;
        }

        public StockDataService StockDataService { get; set; }

        /// <summary>
        /// reads files and then transmits the api responses to the database
        /// </summary>
        public override void Run()
        {
            const string APIKEY = @"EZC8NLKMV664QLL3";
            const string CONNECTIONSTRING = "";
            StockDataService = new StockDataService(APIKEY, CONNECTIONSTRING);
            ValidateFileToProcess();
        }

        /// <summary>
        /// checks whether the file exists or not
        /// </summary>
        public void ValidateFileToProcess()
        {
            string rESXFilePath = string.Empty;
            Console.WriteLine("Geben Sie den Pfad zur Datei an, die ausgelesen werden soll. Die enthaltenen Daten werden anschließend zusammen mit den API Daten in die Datenbank geschrieben");

            while (true)
            {
                rESXFilePath = Console.ReadLine();
                if (File.Exists(rESXFilePath))
                    break;
                else
                    Console.WriteLine("Die Datei im angegebenen Pfad scheint nicht zu existieren. Bitte geben Sie einen gültigen Pfad ein");
            }

            FileReaderBase rESXFiles = FileReaderFactory.GetReader(rESXFilePath);
            rESXFiles.GetFiles().ForEach(async F => await StockDataService.FetchAndStoreStockData(F));
        }
    }
}
