namespace Import.FileHandler
{
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
        }

        /// <summary>
        /// reads the csv file
        /// </summary>
        public override void ReadXMLFiles()
        {
            using (StreamReader xMLReader = new StreamReader(RESXFile))
            {
                string cSVValue = xMLReader.ReadToEnd();
                string[] cSVSplittedValue = cSVValue.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                cSVSplittedValue
                    .ToList()
                    .ForEach(L => Stocks.AddRange(L.Split(',')));

            }
        }
    }
}
