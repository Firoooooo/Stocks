using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Import;
using Import.Singleton;
using MySql.Data.MySqlClient;

namespace StockHTML.SQL
{
    public class SQLHandler
    {
        public MySqlConnection SQLConnection { get; set; }
        public Connections Connections { get; set; }
        public string EMail { get; set; }


        /// <summary>
        /// constructor that receives the context and opens the connection
        /// </summary>
        /// <param name="_cON">context class</param>
        public SQLHandler(Connections _cON, string _eMAIL)
        {
            SQLConnection = new MySqlConnection(_cON.CONNECTIONSTRING);
            SQLConnection.Open();
            Connections = _cON;
            EMail = _eMAIL;
        }

        public void QueryDataFromDB()
        {
            SQLInitializer.ExecuteQuery($"USE {Connections.xInstance.DATABASENAME};"
                , SQLConnection);


            string sQLQuery = $@"SELECT U.USERID, U.USERNAME, U.EMAIL, U.BALANCE, T.TRANSACTIONID, T.TRANSACTIONTYPE, T.QUANTITY AS TRANSACTION_QUANTITY, T.PRICE AS TRANSACTION_PRICE, T.TRANSACTIONDATE, TS.SYMBOL AS TRANSACTION_STOCK_SYMBOL, TS.DATE AS TRANSACTION_STOCK_DATE, TS.CLOSE AS TRANSACTION_STOCK_CLOSE, P.PORTFOLIOID, P.QUANTITY AS PORTFOLIO_QUANTITY, PS.SYMBOL AS PORTFOLIO_STOCK_SYMBOL, PS.CLOSE AS PORTFOLIO_STOCK_CLOSE, PVH.HISTORYID, PVH.TOTALVALUE, PVH.DATE AS VALUE_DATE FROM USER U LEFT JOIN TRANSACTION T ON U.USERID = T.USERID LEFT JOIN STOCK TS ON T.STOCKID = TS.STOCKID LEFT JOIN USERPORTFOLIO P ON U.USERID = P.USERID LEFT JOIN STOCK PS ON P.STOCKID = PS.STOCKID LEFT JOIN PORTFOLIOVALUEHISTORY PVH ON U.USERID = PVH.USERID WHERE U.EMAIL = @EMAIL;";

            using (MySqlTransaction sQLTransaction = SQLConnection.BeginTransaction())
            using (MySqlCommand sQLCommand = new MySqlCommand(sQLQuery, SQLConnection, sQLTransaction))
            {
                try
                {
                    sQLCommand.Parameters.Clear();
                    sQLCommand.Parameters.AddWithValue("@EMAIL", EMail);

                    using (MySqlDataReader sQLReader = sQLCommand.ExecuteReader())
                    {
                        while (sQLReader.Read())
                        {
                            Console.WriteLine("USERID : " + sQLReader["USERID"]);
                            Console.WriteLine("USERNAME : " + sQLReader["USERNAME"]);
                            Console.WriteLine("EMAIL : " + sQLReader["EMAIL"]);
                            Console.WriteLine("BALANCE : " + sQLReader["BALANCE"]);
                            Console.WriteLine("TRANSACTIONID : " + sQLReader["TRANSACTIONID"]);
                            Console.WriteLine("TRANSACTIONTYPE : " + sQLReader["TRANSACTIONTYPE"]);
                            Console.WriteLine("TRANSACTION_QUANTITY : " + sQLReader["TRANSACTION_QUANTITY"]);
                            Console.WriteLine("TRANSACTION_PRICE : " + sQLReader["TRANSACTION_PRICE"]);
                            Console.WriteLine("TRANSACTIONDATE : " + sQLReader["TRANSACTIONDATE"]);
                            Console.WriteLine("TRANSACTION_STOCK_SYMBOL : " + sQLReader["TRANSACTION_STOCK_SYMBOL"]);
                            Console.WriteLine("TRANSACTION_STOCK_DATE : " + sQLReader["TRANSACTION_STOCK_DATE"]);
                            Console.WriteLine("TRANSACTION_STOCK_CLOSE : " + sQLReader["TRANSACTION_STOCK_CLOSE"]);
                            Console.WriteLine("PORTFOLIOID : " + sQLReader["PORTFOLIOID"]);
                            Console.WriteLine("PORTFOLIO_QUANTITY : " + sQLReader["PORTFOLIO_QUANTITY"]);
                            Console.WriteLine("PORTFOLIO_STOCK_SYMBOL : " + sQLReader["PORTFOLIO_STOCK_SYMBOL"]);
                            Console.WriteLine("PORTFOLIO_STOCK_CLOSE : " + sQLReader["PORTFOLIO_STOCK_CLOSE"]);
                            Console.WriteLine("HISTORYID : " + sQLReader["HISTORYID"]);
                            Console.WriteLine("TOTALVALUE : " + sQLReader["TOTALVALUE"]);
                            Console.WriteLine("VALUE_DATE : " + sQLReader["VALUE_DATE"]);
                        }
                    }
                }
                catch (Exception EX)
                {
                    sQLTransaction.Rollback();
                    Console.WriteLine($"{Import.Resources.Labels.DataImportFailed} {EX.Message}");
                }
            }
        }
    }
}
