using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Import.ImportHandler
{
    public class TXT : FileReaderBase
    {
        public string RESXFile { get; set; }

        /// <summary>
        /// constructor  to receive and process the stream
        /// </summary>
        /// <param name="_rESXFile">file stream</param>
        public TXT(string _rESXFile)
        {
            RESXFile = _rESXFile;

            ReadRESXFiles();
        }

        /// <summary>
        /// reads the txt file
        /// </summary>
        public override void ReadRESXFiles()
        {
            using (StreamReader rESXStreamReader = new StreamReader(RESXFile))
            {
                string[] rESXSplitted = rESXStreamReader.ReadToEnd()
                         .Split(new[] { '\r', '\n' },
                                StringSplitOptions.RemoveEmptyEntries);
                rESXSplitted.ToList().ForEach(E => Stocks.Add(E));
            }
        }
    }
}
