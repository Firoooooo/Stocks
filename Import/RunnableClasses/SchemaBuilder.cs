using Import.Singleton;

namespace Import.RunnableClasses
{
    [RunnableClassAttribute(1, "Initialisierung der Datenbankstruktur einschließlich der Tabellen und Referenzen")]
    public class SchemaBuilder : RunnableClassBase
    {
        /// <summary>
        /// run method of the runnable classes
        /// </summary>
        public override void Run()
        {
            SQLInitializer sQLInitializer = new SQLInitializer();
            sQLInitializer.Initialize();
        }
    }
}
