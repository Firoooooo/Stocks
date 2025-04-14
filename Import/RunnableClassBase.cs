using Import.Factories;
using Import.Resources;
using Import.Singleton;
using System.Reflection;

namespace Import
{
    /// <summary>
    /// base class for the runnable classes
    /// </summary>
    public abstract class RunnableClassBase
    {
        public Connections CON { get; set; }
        public string RESXFilePath { get; set; } 
            = string.Empty;

        /// <summary>
        /// constructor that receives the context
        /// </summary>
        public RunnableClassBase(Connections _cON)
        {
            CON = _cON;
        }

        /// <summary>
        /// method that checks whether the file exists or not and returns the file entrys
        /// </summary>
        /// <returns>List<string></returns>
        public FileReaderBase GetRESXReader()
        {
            Console.WriteLine(Labels.EnterFile);

            while (true)
            {
                RESXFilePath = Console.ReadLine();

                if (File.Exists(RESXFilePath))
                {
                    break;
                }
                else
                {
                    var rESXFile = Assembly.GetExecutingAssembly();
                    var rESXFileName = rESXFile.GetManifestResourceNames().FirstOrDefault(X => X.Contains(RESXFilePath));
                    if (rESXFileName != null)
                    {
                        RESXFilePath = rESXFileName;
                        break;
                    }

                    Console.WriteLine(Labels.FileDoesntExists);
                }
            }

            FileReaderBase rESXFiles = FileReaderFactory.GetReader(RESXFilePath);

            return rESXFiles;
        }

        /// <summary>
        /// run method of the runnable classes
        /// </summary>
        public abstract void Run();
    }
}
