using Import.Singleton;

namespace Import.RunnableClasses.Upload
{
    [Upload(3, "Importiert Portfolio Daten und speichert sie in der User Portfolio Tabelle")]
    public class UserPortfolio : RunnableClassBase
    {
        /// <summary>
        /// constructor that receives the context and passed it on to the base class
        /// </summary>
        /// <param name="_cON">connection context</param>
        public UserPortfolio(Connections _cON)
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
            sQLInitializer.InsertInUserPortfolio(rESXFiles.RESXFile);
        }
    }
}
