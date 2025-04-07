using Import.Context;
using MySql.Data.MySqlClient;

namespace Import.RunnableClasses
{
    [RunnableClassAttribute(1, "Initialisierung der Datenbankstruktur einschließlich der Tabellen und Referenzen")]
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
            using (MySqlConnection sQLCon = new MySqlConnection(CON.CONNECTIONSTRING))
            {
                sQLCon.Open();

                SQLInitializer sQLInitializer = new SQLInitializer(CON);
                sQLInitializer.Initialize();
                // Evtl. eine Methode zum befüllen der Tabellen ? 
            }
        }
    }
}
