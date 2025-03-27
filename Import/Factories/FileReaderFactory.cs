using Import.FileHandler;
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
                    return new TXT(_rESXFile);

                case ".CSV":
                    return new CSV(_rESXFile);

                case ".XLSX":
                    return new XLSX(_rESXFile);

                default:
                    throw new ArgumentException(Resources.Labels.NotImplemented);
            }
        }
    }
}
