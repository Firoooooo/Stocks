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
            var rUNNABClasses = Assembly.GetExecutingAssembly().GetTypes()
                            .Where(T => T.GetCustomAttributes(typeof(RunnableClassAttribute), false).Any())
                            .Select(T => new
                            {
                                Type = T,
                                Attribute = (RunnableClassAttribute)T.GetCustomAttributes(typeof(RunnableClassAttribute), false).FirstOrDefault()
                            })
                            .OrderBy(A => A.Attribute.TransactionNumber)
                            .ToList();
            rUNNABClasses.Add(new { Type = typeof(Program), Attribute = new RunnableClassAttribute(rUNNABClasses.Count + 1, "Program beenden") });

            while (true)
            {
                Console.Clear();
                rUNNABClasses.ForEach(T => Console.WriteLine($"{T.Attribute.TransactionNumber} {T.Attribute.Name}"));

                if (int.TryParse(Console.ReadLine(), out int eXCNumber))
                {
                    if (rUNNABClasses.Last().Attribute.TransactionNumber == eXCNumber)
                        break;

                    var sELClass = rUNNABClasses.FirstOrDefault(X => X.Attribute.TransactionNumber == eXCNumber);

                    switch ((OperationTypes)Enum.Parse(typeof(OperationTypes), sELClass.Type.Name))
                    {
                        case OperationTypes.DBImportStacks:
                            DBImportStacks dBImportStacks = new DBImportStacks();
                            dBImportStacks.Run();
                            break;

                        case OperationTypes.ClearStocks:
                            ClearStocks cLEARStocks = new ClearStocks();
                            cLEARStocks.Run();
                            break;
                    }
                }
            }
        }
    }
}