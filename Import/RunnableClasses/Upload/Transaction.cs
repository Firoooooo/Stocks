using Import.Model;
using System.Globalization;
using System.Text.Json.Serialization.Metadata;

namespace Import.RunnableClasses.Upload
{
    [Upload(5, "Importiert Transaktionsdaten und speichert diese in der Transactions Tabelle")]
    public class Transaction : RunnableClassBase
    {
        public static Dictionary<int, DTOTransaction> TransactionMap { get; set; }

        /// <summary>
        /// run method of the runnable classes
        /// </summary>
        public override void Run()
        {
            FileReaderBase rESXFiles = GetRESXReader();
            CreateTransactionMap(rESXFiles.Stocks);
            SQLInitializer sQLInitializer = new SQLInitializer();
            sQLInitializer.InsertInTransaction(rESXFiles.RESXFile);
        }

        /// <summary>
        /// creates a dictionary of transactions
        /// </summary>
        /// <param name="_rESX">resx file</param>
        private void CreateTransactionMap(List<string> _rESX)
        {
            TransactionMap = new Dictionary<int, DTOTransaction>();
            int rESXC = 1;

            _rESX.ForEach(S =>
            {
                string[] rESXCSplit = S.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                TransactionMap.Add(rESXC, new DTOTransaction
                {
                    UserID = int.Parse(rESXCSplit[0]), StockID = int.Parse(rESXCSplit[1]), TransactionType = rESXCSplit[2].ToString(), Quantity = int.Parse(rESXCSplit[3])
                });
                rESXC++;
            });
        }
    }
}
