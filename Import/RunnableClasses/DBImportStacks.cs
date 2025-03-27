using Import.Factories;

namespace Import.RunnableClasses
{
    /// <summary>
    /// Liest Bezeichnungen aus einer Datei, ruft die zugehörigen Daten über eine API ab und speichert sie in einer Datenbank
    /// </summary>
    [RunnableClassAttribute(1, "Liest Bezeichnungen aus einer Datei, ruft die zugehörigen Daten über eine API ab und speichert sie in einer Datenbank")]
    public class DBImportStacks : RunnableClassBase
    {
        public StockDataService StockDataService { get; set; }

        /// <summary>
        /// reads files and then transmits the api responses to the database
        /// </summary>
        public override void Run()
        {
            string aPIKEY = @"EZC8NLKMV664QLL3";
            string cONString = string.Empty;
            StockDataService = new StockDataService(aPIKEY, cONString);
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

            FileReaderBase rESXFiles = FileReaderFactory.GetReader("Import.Files.NASDAQ.txt");
            rESXFiles.GetFiles().ForEach(F => StockDataService.FetchAndStoreStockData(F));
        }
    }
}
