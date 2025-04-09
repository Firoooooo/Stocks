using DocumentFormat.OpenXml.Office.SpreadSheetML.Y2021.ExtLinks2021;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Import.RunnableClasses;
using System.Data;
using System.Reflection;
using Import.Context;
using Import.Resources;
using Org.BouncyCastle.Crypto.Engines;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Configuration;

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
            Connections cON = null;

            using (Stream rESXFile = Assembly.GetExecutingAssembly().GetManifestResourceStream("Import.Configs.Config.json"))
            {
                using (StreamReader rESXReader = new StreamReader(rESXFile))
                {
                    string rESXContent = rESXReader.ReadToEnd();

                    cON = JsonConvert.DeserializeObject<Connections>(rESXContent);
                }
            }

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
                    ExecuteOperation(cON, cLASS.Type);
                }
            }
        }

        /// <summary>
        /// executes he corresponding operation
        /// </summary>
        /// <param name="_cON">context class which includes the informations needed during the operation</param>
        /// <param name="_eXCType">attribute of the class</param>
        private static void ExecuteOperation(Connections _cON, Type _eXCType)
        {
            switch ((OperationTypes)Enum.Parse(typeof(OperationTypes), _eXCType.Name))
            {
                case OperationTypes.DBImportStacks:
                    new DBImportStacks(_cON).Run();
                    break;

                case OperationTypes.ClearStocks:
                    new ClearStocks(_cON).Run();
                    break;

                case OperationTypes.SchemaBuilder:
                    new SchemaBuilder(_cON).Run();
                    break;

                // Weitere Operationen hier hinzufügen, falls notwendig
            }
        }
    }
}