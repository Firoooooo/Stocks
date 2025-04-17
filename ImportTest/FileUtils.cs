using Import;
using Import.Factories;
using Import.Singleton;
using Newtonsoft.Json;
using System.Reflection;

namespace ImportTest
{
    /// <summary>
    /// class to check whether the individual data can be read out
    /// </summary>
    [TestClass]
    public class FileUtils
    {
        /// <summary>
        /// test method to check whether the file can be read
        /// </summary>
        [TestMethod]
        public void ReadTXTFile()
        {
            FileReaderBase rESXBase = FileReaderFactory.GetReader("Import.Resources.Stock.Stock.txt");
            rESXBase.ReadRESXFile();

            Assert.IsNotNull(rESXBase.Stocks);
        }

        /// <summary>
        /// test method to check whether the file can be read
        /// </summary>
        [TestMethod]
        public void ReadCSVFile()
        {
            FileReaderBase rESXBase = FileReaderFactory.GetReader("Import.Resources.Stock.Stock.csv");
            rESXBase.ReadRESXFile();

            Assert.IsNotNull(rESXBase.Stocks);
        }

        /// <summary>
        /// test method to check whether the config is given
        /// </summary>
        [TestMethod]
        public void CheckIfConfigIsValid()
        {
            Connections cON = Connections.xInstance;

            using (Stream rESXBase = Assembly.Load("Import").GetManifestResourceStream("Import.Configs.Config.json"))
            {
                using (StreamReader rESXStreamReader = new StreamReader(rESXBase))
                {
                    string rESXCOntent = rESXStreamReader.ReadToEnd();

                    JsonConvert.PopulateObject(rESXCOntent, cON);
                }
            }

            Assert.IsNotNull(cON.CONNECTIONSTRING);
            Assert.IsNotNull(cON.DATABASENAME);
            Assert.IsNotNull(cON.APIKEY);
        }
    }
}
