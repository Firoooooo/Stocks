using Import.ImportHandler;
using System.Reflection;

namespace Import.Factories
{
    public class FileReaderFactory
    {
        public static FileReaderBase GetReader(string _rESXFile)
        {
            switch (Path.GetExtension(_rESXFile).ToUpper())
            {
                case ".TXT":
                    return new TXT(Assembly.GetExecutingAssembly().GetManifestResourceStream(_rESXFile));

                default:
                    throw new ArgumentException("File Handling ist noch nicht Implementiert");
            }
        }
    }
}
