using ClosedXML.Excel;

namespace Import.FileHandler
{
    /// <summary>
    /// class to read the xlsx file
    /// </summary>
    public class XLSX : FileReaderBase
    {
        public string RESXFile { get; set; }

        /// <summary>
        /// constructor  to receive and process the stream
        /// </summary>
        /// <param name="_rESXFile">file stream</param>
        public XLSX(string _rESXFile)
        {
            RESXFile = _rESXFile;

            ReadRESXFiles();
        }

        /// <summary>
        /// reads the xlsx file
        /// </summary>
        public override void ReadRESXFiles()
        {
            using (var xLSXWorkbook = new XLWorkbook(RESXFile))
            {
                var xLSXWorksheet = xLSXWorkbook.Worksheet(1);
                var xLSXRange = xLSXWorksheet.RangeUsed();
                var xLSXRows = xLSXRange.RowCount();
                var xLSXColumns = xLSXRange.ColumnCount();

                for (int xLSXRow = 1; xLSXRow <= xLSXRows; xLSXRow++)
                {
                    for (int xLSXColumn = 1; xLSXColumn <= xLSXColumns; xLSXColumn++)
                    {
                        if (xLSXRange.Cell(xLSXRow, xLSXColumn).Value.ToString() == "")
                            continue;
                        Stocks.Add(xLSXRange.Cell(xLSXRow, xLSXColumn).Value.ToString());
                    }
                }
            }
        }
    }
}
