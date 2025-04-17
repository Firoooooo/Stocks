using Import.Resources;
using Import.RunnableClasses.Upload;
using Import.Singleton;
using System.Reflection;

namespace Import
{
    [RunnableClassAttribute(2, "Importoptionen anzeigen, um die Dateien in die Ziel Tabellen einzuspielen")]
    public class UploadHandler : RunnableClassBase
    {
        /// <summary>
        /// run method of the runnable classes
        /// </summary>
        public override void Run()
        {
            var rUNNABClasses = Assembly.GetExecutingAssembly().GetTypes()
                            .Where(T => T.GetCustomAttributes(typeof(UploadAttribute), false).Any())
                            .Select(T => new
                            {
                                Type = T,
                                Attribute = (UploadAttribute)T.GetCustomAttributes(typeof(UploadAttribute), false).FirstOrDefault()
                            })
                            .OrderBy(A => A.Attribute.TransactionNumber)
                            .ToList();
            rUNNABClasses.Add(new { Type = typeof(Program), Attribute = new UploadAttribute(rUNNABClasses.Count + 1, Labels.EndProgram) });

            while (true)
            {
                Console.Clear();
                rUNNABClasses.ForEach(T => Console.WriteLine($"{T.Attribute.TransactionNumber} {T.Attribute.Name}"));

                if (int.TryParse(Console.ReadLine(), out int eXCNumber))
                {
                    if (rUNNABClasses.Last().Attribute.TransactionNumber == eXCNumber)
                        break;

                    var cLASS = rUNNABClasses.FirstOrDefault(X => X.Attribute.TransactionNumber == eXCNumber);
                    ExecuteOperation(Connections.xInstance, cLASS.Type);
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
                case OperationTypes.StockAPI:
                    new StockAPI().Run();
                    break;

                case OperationTypes.Stock:
                    new Stock().Run();
                    break;

                case OperationTypes.User:
                    new User().Run();
                    break;

                case OperationTypes.Transaction:
                    new Transaction().Run();
                    break;

                case OperationTypes.UserPortfolio:
                    new UserPortfolio().Run();
                    break;

                case OperationTypes.PortfolioValueHistory:
                    new PortfolioValueHistory().Run();
                    break;
            }
        }
    }
}
