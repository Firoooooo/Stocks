namespace Import
{
    /// <summary>
    /// base class for the file handling
    /// </summary>
    public abstract class FileReaderBase
    {
        public List<string> Stocks { get; set; } 
            = new List<string>();

        /// <summary>
        /// returns the curr stocks
        /// </summary>
        /// <returns>List<string></returns>
        public List<string> GetFiles() => Stocks;

        public abstract void ReadXMLFiles();
    }
}
