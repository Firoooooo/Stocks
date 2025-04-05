using DocumentFormat.OpenXml.Office.SpreadSheetML.Y2021.ExtLinks2021;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Import.RunnableClasses;
using System.Data;
using System.Reflection;
using Import.Context;
using Import.Resources;

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
        static void Main(string[] _aRGS)
        {
            // Bei der Auführung muss der Connection Sting zur Datenbank angegeben werden. Die API kann weiterhin über den API Key angesprochen werden, ist allerdings auf 25 Anfragen pro Tag beschränkt
            Connections cON = new Connections("EZC8NLKMV664QLL3", "Server=localhost;Port=3306;User Id=root;Password=Password;", "Stocks");

            var rUNNABClasses = Assembly.GetExecutingAssembly().GetTypes()
                            .Where(T => T.GetCustomAttributes(typeof(RunnableClassAttribute), false).Any())
                            .Select(T => new
                            {
                                Type = T,
                                Attribute = (RunnableClassAttribute)T.GetCustomAttributes(typeof(RunnableClassAttribute), false).FirstOrDefault()
                            })
                            .OrderBy(A => A.Attribute.TransactionNumber)
                            .ToList();
            rUNNABClasses.Add(new { Type = typeof(Program), Attribute = new RunnableClassAttribute(rUNNABClasses.Count + 1, Labels.EndProgram) });

            while (true)
            {
                Console.Clear();
                rUNNABClasses.ForEach(T => Console.WriteLine($"{T.Attribute.TransactionNumber} {T.Attribute.Name}"));

                if (int.TryParse(Console.ReadLine(), out int eXCNumber))
                {
                    if (rUNNABClasses.Last().Attribute.TransactionNumber == eXCNumber)
                        break;

                    var cLASS = rUNNABClasses.FirstOrDefault(X => X.Attribute.TransactionNumber == eXCNumber);

                    switch ((OperationTypes)Enum.Parse(typeof(OperationTypes), cLASS.Type.Name))
                    {
                        case OperationTypes.DBImportStacks:
                            DBImportStacks dBImportStacks = new DBImportStacks(cON);
                            dBImportStacks.Run();
                            break;

                        case OperationTypes.ClearStocks:
                            ClearStocks cLEARStocks = new ClearStocks(cON);
                            cLEARStocks.Run();
                            break;

                        case OperationTypes.SchemaBuilder:
                            SchemaBuilder sCHEmaBuilder = new SchemaBuilder(cON);
                            sCHEmaBuilder.Run();
                            break;
                    }
                }
            }
        }
    }
}