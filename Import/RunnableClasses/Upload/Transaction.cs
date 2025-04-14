namespace Import.RunnableClasses.Upload
{
    [Upload(4, "Importiert Transaktionsdaten und speichert diese in der Transactions Tabelle")]
    public class Transaction : RunnableClassBase
    {
        /// <summary>
        /// run method of the runnable classes
        /// </summary>
        public override void Run()
        {
            FileReaderBase rESXFiles = GetRESXReader();
            SQLInitializer sQLInitializer = new SQLInitializer();
            sQLInitializer.InsertInTransaction(rESXFiles.RESXFile);
        }
    }
}
