using DocumentFormat.OpenXml.Office.SpreadSheetML.Y2021.ExtLinks2021;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Import.RunnableClasses;
using System.Data;
using System.Reflection;

namespace Import
{
    /// <summary>
    /// program class that executes the functions
    /// </summary>
    public class Program
    {
        /// <summary>
        /// main mehtode that executes the api call, prepares the data and writes it to the database
        /// </summary>
        /// <param name="_aRGS">args</param>
        /// <returns>Task</returns>
        static void Main(string[] _aRGS)
        {
            Operation oP = new Operation();
            oP.RunSelectedTask();
        }
    }
}