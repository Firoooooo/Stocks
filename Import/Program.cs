using Import.Factories;

namespace Import
{
    public class Program
    {
        static async Task Main(string[] _Args)
        {
            string aPIKEY = @"EZC8NLKMV664QLL3";
            string cONString = string.Empty;
            StockDataService sTockDataService = new StockDataService(aPIKEY, cONString);

            FileReaderBase rESXFiles = FileReaderFactory.GetReader("Import.Files.NASDAQ.txt");
            rESXFiles.GetFiles().ForEach(F => sTockDataService.FetchAndStoreStockData(F));
        }
    }
}