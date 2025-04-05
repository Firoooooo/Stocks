using Import.Context;

namespace Import.RunnableClasses
{
    /// <summary>
    /// class that deletes the entries of the shares in the database
    /// </summary>
    [RunnableClassAttribute(3, "Löscht die Einträge zu den Aktien aus der Datenbank")]
    public class ClearStocks : RunnableClassBase
    {
        /// <summary>
        /// base call
        /// </summary>
        /// <param name="_cON">connection context</param>
        public ClearStocks(Connections _cON) 
            : base(_cON)
        {
            CON = _cON;
        }

        /// <summary>
        /// run method of the runnable classes
        /// </summary>
        public override void Run()
        {

        }
    }
}
