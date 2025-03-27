using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class RunnableClassAttribute : Attribute
    {
        public int TransactionNumber { get; set; }  
        public string Name { get; set; }

        public RunnableClassAttribute(int _tRANSNum, string _nAME) 
        {
            TransactionNumber = _tRANSNum;
            Name = _nAME;
        }
    }
}
