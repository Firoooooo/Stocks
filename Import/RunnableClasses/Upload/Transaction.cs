using Import.Singleton;

namespace Import.RunnableClasses.Upload
{
    [Upload(4, "Importiert Transaktionsdaten und speichert diese in der Transactions Tabelle")]
    public class Transaction : RunnableClassBase
    {
        /// <summary>
        /// constructor that receives the context and passed it on to the base class
        /// </summary>
        /// <param name="_cON">connection context</param>
        public Transaction(Connections _cON)
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
            sQLInitializer.InsertInTransaction(rESXFiles.RESXFile);
        }
    }
}
