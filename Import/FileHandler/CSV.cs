using System.Reflection;

namespace Import.FileHandler
{
    /// <summary>
    /// class to read the csv file
    /// </summary>
    public class CSV : FileReaderBase
    {
        /// <summary>
        /// constructor to receive and process the stream
        /// </summary>
        /// <param name="_rESXFile">file stream</param>
        public CSV(string _rESXFile)
        {
            RESXFile = _rESXFile;

            ReadRESXFile();
        }

        /// <summary>
        /// reads the csv file
        /// </summary>
        public override void ReadRESXFile()
        {
            if (File.Exists(RESXFile))
            {
                using (StreamReader rESXStreamReader = new StreamReader(RESXFile))
                {
                    string rESXContent = rESXStreamReader.ReadToEnd();
                    ParseAndAddStocks(rESXContent);
                }
            }
            else
            {
                using (Stream rESXFile = Assembly.GetExecutingAssembly().GetManifestResourceStream(RESXFile))
                {
                    using (StreamReader rESXReader = new StreamReader(rESXFile))
                    {
                        string rESXContent = rESXReader.ReadToEnd();
                        ParseAndAddStocks(rESXContent);
                    }
                }
            }
        }

        /// <summary>
        /// parses the csv file and adds the stocks to the list
        /// </summary>
        /// <param name="_rESXContent"></param>
        private void ParseAndAddStocks(string _rESXContent)
        {
            _rESXContent = _rESXContent.Replace("\"", "").Replace("\"\"", "");
            string[] rESXSplitted = _rESXContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            rESXSplitted
                .Where(V => !String.IsNullOrWhiteSpace(V))
                .Select(V => V.Replace(",", ""))
                .ToList()
                .ForEach(V => Stocks.Add(V));
        }

        /// <summary>
        /// reads the files and then transmits it to the database
        /// </summary>
        public void ReadRESXFiles()
        {

        }
    }
}
