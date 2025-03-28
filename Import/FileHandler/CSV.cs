namespace Import.FileHandler
{
    /// <summary>
    /// class to read the csv file
    /// </summary>
    public class CSV : FileReaderBase
    {
        public string RESXFile { get; set; }

        /// <summary>
        /// constructor  to receive and process the stream
        /// </summary>
        /// <param name="_rESXFile">file stream</param>
        public CSV(string _rESXFile)
        {
            RESXFile = _rESXFile;

            ReadRESXFiles();
        }

        /// <summary>
        /// reads the csv file
        /// </summary>
        public override void ReadRESXFiles()
        {
            using (StreamReader xMLReader = new StreamReader(RESXFile))
            {
                string cSVValue = xMLReader.ReadToEnd();
                cSVValue = cSVValue.Replace("\"", "").Replace("\"\"", "");
                string[] cSVSplittedValue = cSVValue.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                cSVSplittedValue
                    .Where(V => !String.IsNullOrWhiteSpace(V))
                    .Select(V => V.Replace(",", ""))
                    .ToList()
                    .ForEach(V => Stocks.Add(V));

            }
        }
    }
}
