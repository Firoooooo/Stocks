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
        static async Task Main(string[] _aRGS)
        {
            RunSelectedTask();
        }

        /// <summary>
        /// runs the selected task
        /// </summary>
        private static void RunSelectedTask()
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

            Console.WriteLine("Entschiede dich für den Job, der ausgeführt werden soll");
            rUNNABClasses.ForEach(T => Console.WriteLine($"{T.Attribute.TransactionNumber} {T.Attribute.Name}"));

            if (int.TryParse(Console.ReadLine(), out int eXCNumber))
            {
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