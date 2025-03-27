using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import
{
    /// <summary>
    /// the attribute that identifies a runnable class so that they can be collected and then displayed together in an execztion window
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RunnableClassAttribute : Attribute
    {
        public int TransactionNumber { get; set; }  
        public string Name { get; set; }

        /// <summary>
        /// extend the constructor with which the classes must be initialized
        /// </summary>
        /// <param name="_tRANSNum">transaction number</param>
        /// <param name="_nAME">name of the job</param>
        public RunnableClassAttribute(int _tRANSNum, string _nAME) 
        {
            TransactionNumber = _tRANSNum;
            Name = _nAME;
        }
    }
}
