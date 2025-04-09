using Import.Context;

namespace Import.RunnableClasses
{
    [RunnableClassAttribute(1, "Initialisierung der Datenbankstruktur einschließlich der Tabellen und Referenzen")]
    public class SchemaBuilder : RunnableClassBase
    {
        public Connections CON { get; set; }

         
        /// <summary>
        /// constructor that receives the context and passed it on to the base class
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
            SQLInitializer sQLInitializer = new SQLInitializer(CON);
            sQLInitializer.Initialize();
        }
    }
}
