using System.Reflection;

namespace Import.ImportHandler
{
    /// <summary>
    /// class to read the txt file
    /// </summary>
    public class TXT : FileReaderBase
    {
        /// <summary>
        /// constructor  to receive and process the stream
        /// </summary>
        /// <param name="_rESXFile">file stream</param>
        public TXT(string _rESXFile)
        {
            RESXFile = _rESXFile;

            ReadRESXFile();
        }

        /// <summary>
        /// reads the txt file
        /// </summary>
        public override void ReadRESXFile()
        {
            if (File.Exists(RESXFile))
            {
                using (StreamReader rESXStreamReader = new StreamReader(RESXFile))
                {
                    ParseAndAddStocks(rESXStreamReader);
                }
            }
            else
            {
                using (Stream rESXFile = Assembly.GetExecutingAssembly().GetManifestResourceStream(RESXFile))
                {
                    using (StreamReader rESXStreamReader = new StreamReader(rESXFile))
                    {
                        ParseAndAddStocks(rESXStreamReader);
                    }
                }
            }
        }

        /// <summary>
        /// parses the txt file and adds the stocks to the list
        /// </summary>
        /// <param name="_rESXStreamReader">ressource stream</param>
        private void ParseAndAddStocks(StreamReader _rESXStreamReader)
        {
            string[] rESXSplitted = _rESXStreamReader.ReadToEnd()
                                        .Split("\r\n",
                                               StringSplitOptions.RemoveEmptyEntries);
            rESXSplitted.ToList().ForEach(E => Stocks.Add(E));
        }
    }
}
