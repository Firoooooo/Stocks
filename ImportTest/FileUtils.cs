using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Import;
using Import.Factories;

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
            FileReaderBase rESXBase = FileReaderFactory.GetReader("Import.Files.Stock.Stock.txt");
            rESXBase.ReadRESXFile();

            Assert.IsNotNull(rESXBase.Stocks);
        }

        /// <summary>
        /// test method to check whether the file can be read
        /// </summary>
        [TestMethod]
        public void ReadCSVFile()
        {
            FileReaderBase rESXBase = FileReaderFactory.GetReader("Import.Files.Stock.Stock.csv");
            rESXBase.ReadRESXFile();

            Assert.IsNotNull(rESXBase.Stocks);
        }
    }
}
