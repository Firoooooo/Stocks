using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Import.Context;

namespace Import.RunnableClasses
{
    [RunnableClassAttribute(2, "Erstellen der Datenbankstruktur einschließlich der Tabellen und Referenzen")]
    public class SchemaBuilder : RunnableClassBase
    {
        /// <summary>
        /// base call
        /// </summary>
        /// <param name="_cON">connection context</param>
        public SchemaBuilder(Connections _cON) 
            : base(_cON)
        {
            CON = _cON;
        }

        /// <summary>
        /// run method of the runnable classes
        /// </summary>
        public override void Run()
        {
            // TODO : Implementiere den Schema Builder, in dem ein Relationales Datenbankmodell erstellt wird
        }
    }
}
