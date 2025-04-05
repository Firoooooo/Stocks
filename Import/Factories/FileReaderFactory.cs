using Import.FileHandler;
using Import.ImportHandler;
using Import.Resources;

namespace Import.Factories
{
    /// <summary>
    /// factory class to return the appropriate reader based on the file extension
    /// </summary>
    public class FileReaderFactory
    {
        /// <summary>
        /// based on the file extension, returns the appropriate reader
        /// </summary>
        /// <param name="_rESXFile">resx file path</param>
        /// <returns>FileReaderBase</returns>
        /// <exception cref="ArgumentException"></exception>
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
                    throw new ArgumentException(Labels.FileHandlingException);
            }
        }
    }
}
