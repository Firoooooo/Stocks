using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Import.Singleton
{
    /// <summary>
    /// singleton class to ensure that only one instance of the class is created
    /// </summary>
    /// <typeparam name="T">generic parameter</typeparam>
    public abstract class SingletonBase<T> where T : SingletonBase<T>, new()
    {
        private static readonly Lazy<T> Instance = new Lazy<T>(() => new T());
        public static T xInstance => Instance.Value;


        /// <summary>
        /// constructor that checks if an instance has been created and throws an exception if so
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        protected SingletonBase()
        {
            if (Instance.IsValueCreated)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
