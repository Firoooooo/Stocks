using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Import.FileHandler
{
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
        }

        /// <summary>
        /// reads the xlsx file
        /// </summary>
        public override void ReadXMLFiles()
        {
            Microsoft.Office.Interop.Excel.Application xLSX = new Microsoft.Office.Interop.Excel.Application();
            Workbook xLSXWorkbook = xLSX.Workbooks.Open(RESXFile);
            Worksheet xLSXWorksheet = (Worksheet)xLSX.Worksheets[1];
            Microsoft.Office.Interop.Excel.Range xLSXRange = xLSXWorksheet.UsedRange;
            int xLSXRows = xLSXRange.Rows.Count;
            int xLSXColumns = xLSXRange.Columns.Count;

            try
            {
                for (int xLSXRow = 1; xLSXRow <= xLSXRows; xLSXRow++)
                {
                    for (int xLSXColumn = 1; xLSXColumn <= xLSXColumns; xLSXColumn++)
                    {
                        if (((xLSXRange.Cells[xLSXRow, xLSXColumn] as Microsoft.Office.Interop.Excel.Range).Text.ToString()) == "")
                            continue;
                        Stocks.Add((xLSXRange.Cells[xLSXRow, xLSXColumn] as Microsoft.Office.Interop.Excel.Range).Text.ToString());
                    }
                }
                xLSXWorkbook.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                xLSX.Quit();
                Marshal.ReleaseComObject(xLSX);
            }
        }
    }
}
