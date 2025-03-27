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
        public Stream Stream { get; set; }


        public TXT(Stream rESXStream)
        {
            Stream = rESXStream;

            using (StreamReader rESXStreamReader = new StreamReader(Stream))
            {
                string[] rESXSplitted = rESXStreamReader.ReadToEnd()
                         .Split(new[] { '\r', '\n' },
                                StringSplitOptions.RemoveEmptyEntries);
                rESXSplitted.ToList().ForEach(E => Stocks.Add(E));
            }
        }
    }
}
