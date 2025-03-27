using Import.Factories;
using Import.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine(Labels.GiveFileInput);

            while (true)
            {
                rESXFilePath = Console.ReadLine();
                if (File.Exists(rESXFilePath))
                    break;
                else
                    Console.WriteLine(Labels.FileDoesntExists);
            }

            FileReaderBase rESXFiles = FileReaderFactory.GetReader("Import.Files.NASDAQ.txt");
            rESXFiles.GetFiles().ForEach(F => StockDataService.FetchAndStoreStockData(F));
        }
    }
}
