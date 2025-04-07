using Org.BouncyCastle.Asn1.Mozilla;
using Import.Context;

namespace Import
{
    /// <summary>
    /// base class for the runnable classes
    /// </summary>
    public abstract class RunnableClassBase
    {
        public Connections CON { get; set; }


        /// <summary>
        /// constructor
        /// </summary>
        public RunnableClassBase(Connections _cON)
        {
            CON = _cON;
        }

        /// <summary>
        /// run method of the runnable classes
        /// </summary>
        public abstract void Run();

        
    }
}
