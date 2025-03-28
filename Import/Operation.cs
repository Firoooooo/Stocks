using Import.RunnableClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Import
{
    public class Operation
    {
        /// <summary>
        /// runs the selected task
        /// </summary>
        public void RunSelectedTask()
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
            rUNNABClasses.Add(new { Type = typeof(Program), Attribute = new RunnableClassAttribute(rUNNABClasses.Count + 1, "Program beenden")});

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
