namespace Import
{
    public abstract class FileReaderBase
    {
        public List<string> Stocks { get; set; } 
            = new List<string>();

        public List<string> GetFiles() => Stocks;
    }
}
