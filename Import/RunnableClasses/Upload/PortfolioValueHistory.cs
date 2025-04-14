using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Import;
using Import.Singleton;

namespace Import.RunnableClasses.Upload
{
    [Upload(5, "Importiert Historische Portflio Werte und speichert diese in der Portfolio Value History Tabelle")]
    public class PortfolioValueHistory : RunnableClassBase
    {
        /// <summary>
        /// run method of the runnable classes
        /// </summary>
        public override void Run()
        {
            FileReaderBase rESXFiles = GetRESXReader();
            SQLInitializer sQLInitializer = new SQLInitializer();
            sQLInitializer.InsertInPortfolioValueHistory(rESXFiles.RESXFile);
        }
    }
}
