using Import.Resources;
using Import.RunnableClasses;
using Import.Singleton;
using Newtonsoft.Json;
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
        public static void Main(string[] _aRGS)
        {
            Connections cON = Connections.xInstance;

            using (Stream rESXFile = Assembly.GetExecutingAssembly().GetManifestResourceStream("Import.Configs.Config.json"))
            {
                using (StreamReader rESXReader = new StreamReader(rESXFile))
                {
                    string rESXContent = rESXReader.ReadToEnd();

                    JsonConvert.PopulateObject(rESXContent, cON);
                }
            }

            var rUNNABClasses = Assembly.GetExecutingAssembly().GetTypes()
                            .Where(T => T.GetCustomAttributes(typeof(RunnableClassAttribute), false).Any())
                            .Select(T => new TypeWithAttribute
                            {
                                Type = T,
                                Attribute = (RunnableClassAttribute)T.GetCustomAttributes(typeof(RunnableClassAttribute), false).FirstOrDefault()
                            })
                            .OrderBy(A => A.Attribute.TransactionNumber)
                            .ToList();
            rUNNABClasses.Add(new TypeWithAttribute{ Type = typeof(Program), Attribute = new RunnableClassAttribute(rUNNABClasses.Count + 1, Labels.EndProgram) });

            while (true)
            {
                Console.Clear();
                rUNNABClasses.ForEach(T => Console.WriteLine($"{T.Attribute.TransactionNumber} {T.Attribute.Name}"));

                if (int.TryParse(Console.ReadLine(), out int eXCNumber))
                {
                    if (rUNNABClasses.Last().Attribute.TransactionNumber == eXCNumber)
                        break;

                    var cLASS = rUNNABClasses.FirstOrDefault(X => X.Attribute.TransactionNumber == eXCNumber);
                    ExecuteOperation(cLASS.Type);
                }
            }
        }

        /// <summary>
        /// executes he corresponding operation
        /// </summary>
        /// <param name="_cON">context class which includes the informations needed during the operation</param>
        /// <param name="_eXCType">attribute of the class</param>
        private static void ExecuteOperation(Type _eXCType)
        {
            switch ((OperationTypes)Enum.Parse(typeof(OperationTypes), _eXCType.Name))
            {
                case OperationTypes.SchemaBuilder:
                    new SchemaBuilder().Run();
                    break;

                case OperationTypes.UploadHandler:
                    new UploadHandler().Run();
                    break;
            }
        }
    }
}