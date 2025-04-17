using Import.Model;
using System.Globalization;
using System.Runtime.Versioning;

namespace Import.RunnableClasses.Upload
{
    [Upload(4, "Importiert Portfolio Daten und speichert sie in der User Portfolio Tabelle")]
    public class UserPortfolio : RunnableClassBase
    {
        public static Dictionary<int, DTOPortfolio> UserPortfolioMap { get; set; }

        /// <summary>
        /// run method of the runnable classes
        /// </summary>
        public override void Run()
        {
            FileReaderBase rESXFiles = GetRESXReader();
            CreateUserPortfolioMap(rESXFiles.Stocks);
            SQLInitializer sQLInitializer = new SQLInitializer();
            sQLInitializer.InsertInUserPortfolio(rESXFiles.RESXFile);
        }

        /// <summary>
        /// creates a dictionary of user portfolios
        /// </summary>
        /// <param name="_rESX">resx file</param>
        public void CreateUserPortfolioMap(List<string> _rESX)
        {
            UserPortfolioMap = new Dictionary<int, DTOPortfolio>();
            int rESXC = 1;

            _rESX.ForEach(S =>
            {
                string[] rESXCSplit = S.Split(new[] { ' ', '\t', ','});
                UserPortfolioMap.Add(rESXC, new DTOPortfolio
                {
                    UserID = int.Parse(rESXCSplit[0]), StockID = int.Parse(rESXCSplit[1]), Quantity = int.Parse(rESXCSplit[2])
                });
                rESXC++;
            });
        }
    }
}
