namespace Import.RunnableClasses.Upload
{
    [Upload(3, "Importiert Portfolio Daten und speichert sie in der User Portfolio Tabelle")]
    public class UserPortfolio : RunnableClassBase
    {
        /// <summary>
        /// run method of the runnable classes
        /// </summary>
        public override void Run()
        {
            FileReaderBase rESXFiles = GetRESXReader();
            SQLInitializer sQLInitializer = new SQLInitializer();
            sQLInitializer.InsertInUserPortfolio(rESXFiles.RESXFile);
        }
    }
}
