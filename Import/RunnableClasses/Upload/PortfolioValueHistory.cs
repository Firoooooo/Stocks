using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Import;
using Import.Context;

namespace Import.RunnableClasses.Upload
{
    [Upload(5, "Importiert Historische Portflio Werte und speichert diese in der Portfolio Value History Tabelle")]
    public class PortfolioValueHistory : RunnableClassBase
    {
        /// <summary>
        /// constructor that receives the context and passed it on to the base class
        /// </summary>
        /// <param name="_cON">connection context</param>
        public PortfolioValueHistory(Connections _cON)
            : base(_cON)
        {
            CON = _cON;
        }

        /// <summary>
        /// run method of the runnable classes
        /// </summary>
        public override void Run()
        {
            FileReaderBase rESXFiles = GetRESXReader();
            SQLInitializer sQLInitializer = new SQLInitializer(CON);
            sQLInitializer.InsertInPortfolioValueHistory(rESXFiles.RESXFile);
        }
    }
}
