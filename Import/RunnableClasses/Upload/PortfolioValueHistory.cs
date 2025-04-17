using Import.Model;
using System.Globalization;

namespace Import.RunnableClasses.Upload
{
    [Upload(6, "Importiert Historische Portflio Werte und speichert diese in der Portfolio Value History Tabelle")]
    public class PortfolioValueHistory : RunnableClassBase
    {
        public static Dictionary<int, DTOPortfolioValueHistory> PortfolioValueHistoryMap { get; set; }

        /// <summary>
        /// run method of the runnable classes
        /// </summary>
        public override void Run()
        {
            FileReaderBase rESXFiles = GetRESXReader();
            CreatePortfolioValueHistory(rESXFiles.Stocks);
            SQLInitializer sQLInitializer = new SQLInitializer();
            sQLInitializer.InsertInPortfolioValueHistory(rESXFiles.RESXFile);
        }

        /// <summary>
        /// creates a dictionary of portfolio values
        /// </summary>
        /// <param name="_rESX">resx file</param>
        public void CreatePortfolioValueHistory(List<string> _rESX)
        {
            PortfolioValueHistoryMap = new Dictionary<int, DTOPortfolioValueHistory>();
            int rESXC = 1;
            _rESX.ForEach(S =>
            {
                string[] rESXCSplit = S.Split(new[] { ' ', '\t', ',' });
                PortfolioValueHistoryMap.Add(rESXC, new DTOPortfolioValueHistory
                {
                    UserID = int.Parse(rESXCSplit[0]), TotalValue = decimal.Parse((rESXCSplit[1]))
                });
                rESXC++;
            });
        }
    }
}
