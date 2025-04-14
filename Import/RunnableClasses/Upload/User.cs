using Import.Singleton;

namespace Import.RunnableClasses.Upload
{
    [Upload(2, "Importiert Benutzerdaten und speichert diese in der User Tabelle")]
    public class User : RunnableClassBase
    {
        /// <summary>
        /// run method of the runnable classes
        /// </summary>
        public override void Run()
        {
            FileReaderBase rESXFiles = GetRESXReader();
            SQLInitializer sQLInitializer = new SQLInitializer();
            sQLInitializer.InsertInUser(rESXFiles.RESXFile);
        }
    }
}
