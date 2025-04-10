using Import.Context;

namespace Import.RunnableClasses.Upload
{
    [Upload(2, "Importiert Benutzerdaten und speichert diese in der User Tabelle")]
    public class User : RunnableClassBase
    {
        /// <summary>
        /// constructor that receives the context and passed it on to the base class
        /// </summary>
        /// <param name="_cON">connection context</param>
        public User(Connections _cON)
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
            sQLInitializer.InsertInUser(rESXFiles.RESXFile);
        }
    }
}
