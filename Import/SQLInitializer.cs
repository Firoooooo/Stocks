using Import.RunnableClasses.Upload;
using Import.Singleton;
using MySql.Data.MySqlClient;

namespace Import
{
    public class SQLInitializer
    {
        public MySqlConnection SQLConnection { get; set; }


        /// <summary>
        /// constructor that receives the context and opens the connection
        /// </summary>
        /// <param name="_cON">context class</param>
        public SQLInitializer()
        {
            SQLConnection = new MySqlConnection(Connections.xInstance.CONNECTIONSTRING);
            SQLConnection.Open();
        }

        /// <summary>
        /// create database
        /// </summary>
        private void CreateDatabase()
        {
            ExecuteQuery($@"
                CREATE DATABASE IF NOT EXISTS {Connections.xInstance.DATABASENAME};
                ", SQLConnection);
        }

        /// <summary>
        /// create tables in the corresponding database
        /// </summary>
        public void Initialize()
        {
            CreateDatabase();
            ExecuteQuery($"USE {Connections.xInstance.DATABASENAME};"
                , SQLConnection);

            ExecuteQuery(@"
                CREATE TABLE IF NOT EXISTS USER (
                    USERID INT AUTO_INCREMENT PRIMARY KEY,           
                    USERNAME VARCHAR(255) NOT NULL,                  
                    EMAIL VARCHAR(255) NOT NULL,  
                    PASSWORD VARCHAR(255) NOT NULL, 
                    PASSWORDHASH VARCHAR(255) NOT NULL,             
                    BALANCE DECIMAL(15, 2) DEFAULT 0.00              
                );", SQLConnection);

            ExecuteQuery(@"
                CREATE TABLE IF NOT EXISTS Stock (
                    STOCKID INT AUTO_INCREMENT PRIMARY KEY,
                    SYMBOL VARCHAR(10) NOT NULL UNIQUE,
                    DATE DATE NOT NULL,
                    OPEN DECIMAL(10,2) NOT NULL,
                    HIGH DECIMAL(10,2) NOT NULL,
                    LOW DECIMAL(10,2) NOT NULL,
                    CLOSE DECIMAL(10,2) NOT NULL,
                    VOLUME BIGINT NOT NULL,
                    LASTUPDATED TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                    UNIQUE(SYMBOL, DATE) 
                );", SQLConnection);

            ExecuteQuery(@"
                CREATE TABLE IF NOT EXISTS TRANSACTION (
                    TRANSACTIONID INT AUTO_INCREMENT PRIMARY KEY,    
                    USERID INT NOT NULL,                              
                    STOCKID INT NOT NULL,                             
                    TRANSACTIONTYPE VARCHAR(4) NOT NULL,     
                    QUANTITY INT NOT NULL,                            
                    PRICE DECIMAL(10,2) NOT NULL,                    
                    TRANSACTIONDATE TIMESTAMP DEFAULT CURRENT_TIMESTAMP, 
                    FOREIGN KEY (USERID) REFERENCES `USER`(USERID),    
                    FOREIGN KEY (STOCKID) REFERENCES `STOCK`(STOCKID),
                    CHECK (TRANSACTIONTYPE IN ('BUY', 'SELL'))
                );", SQLConnection);

            ExecuteQuery(@"
                CREATE TABLE IF NOT EXISTS USERPORTFOLIO (
                    PORTFOLIOID INT AUTO_INCREMENT PRIMARY KEY,       
                    USERID INT NOT NULL,                              
                    STOCKID INT NOT NULL,                             
                    QUANTITY INT NOT NULL,                            
                    FOREIGN KEY (USERID) REFERENCES `USER`(USERID),    
                    FOREIGN KEY (STOCKID) REFERENCES `STOCK`(STOCKID), 
                    UNIQUE(USERID, STOCKID)
                );", SQLConnection);

            ExecuteQuery(@"
                CREATE TABLE IF NOT EXISTS PORTFOLIOVALUEHISTORY (
                    HISTORYID INT AUTO_INCREMENT PRIMARY KEY,        
                    USERID INT NOT NULL,                              
                    TOTALVALUE DECIMAL(15,2) NOT NULL,               
                    DATE TIMESTAMP DEFAULT CURRENT_TIMESTAMP,         
                    FOREIGN KEY (USERID) REFERENCES `USER`(USERID)     
                );", SQLConnection);
        }

        /// <summary>
        /// executes the query
        /// </summary>
        /// <param name="_sQLQuery">query to execute</param>
        /// <param name="_sQLCon">connection string</param>
        public static void ExecuteQuery(string _sQLQuery, MySqlConnection _sQLCon)
        {
            try
            {
                using (MySqlTransaction sQLTransaction = _sQLCon.BeginTransaction())
                using (MySqlCommand sQLCom = new MySqlCommand(_sQLQuery, _sQLCon))
                {
                    sQLCom.ExecuteNonQuery();
                    sQLTransaction.Commit();
                }
            }
            catch (Exception EX)
            {
                Console.WriteLine($"Error: {EX.Message}");
            }
        }

        /// <summary>
        /// returns the secure file path
        /// </summary>
        /// <param name="_rESXFile">file path</param>
        /// <returns>string</returns>
        public string GetSecureFilePriv(string _rESXFile)
        {
            string sQLQuery = "SHOW VARIABLES LIKE 'SECURE_FILE_PRIV';";
            using (MySqlCommand sQLCommand = new MySqlCommand(sQLQuery, SQLConnection))
            {
                using (MySqlDataReader sQLReader = sQLCommand.ExecuteReader())
                {
                    if (sQLReader.Read())
                    {
                        return sQLReader.GetString(1);
                    }
                }
            }

            return String.Empty;
        }

        /// <summary>
        /// inserts the data into the user table
        /// </summary>
        /// <param name="_rESXFile">file name</param>
        public void InsertInUser(string _rESXFile)
        {
            ExecuteQuery($"USE {Connections.xInstance.DATABASENAME};"
                , SQLConnection);

            string sQLQuery = @"INSERT INTO User (USERNAME, EMAIL, PASSWORD, PASSWORDHASH, BALANCE) VALUES (@USERNAME, @EMAIL, @PASSWORD, @PASSWORDHASH, @BALANCE) ON DUPLICATE KEY UPDATE USERNAME = @USERNAME, EMAIL = @EMAIL, PASSWORD = @PASSWORD, PASSWORDHASH = @PASSWORDHASH, BALANCE = @BALANCE;";

            using (MySqlTransaction sQLTransaction = SQLConnection.BeginTransaction())
            using (MySqlCommand sQLCommand = new MySqlCommand(sQLQuery, SQLConnection, sQLTransaction))
            {
                try
                {
                    User.UserMap.ToList().ForEach(U =>
                    {
                        sQLCommand.Parameters.Clear();

                        sQLCommand.Parameters.AddWithValue("@USERNAME", U.Value.Username);
                        sQLCommand.Parameters.AddWithValue("@EMAIL", U.Value.EMail);
                        sQLCommand.Parameters.AddWithValue("@PASSWORD", U.Value.Password);
                        sQLCommand.Parameters.AddWithValue("@PASSWORDHASH", U.Value.PasswordHash);
                        sQLCommand.Parameters.AddWithValue("@BALANCE", U.Value.Balance);

                        sQLCommand.ExecuteNonQuery();
                    });

                    sQLTransaction.Commit();
                }
                catch (Exception EX)
                {
                    sQLTransaction.Rollback();
                    Console.WriteLine($"{Import.Resources.Labels.DataImportFailed} {EX.Message}");
                }
            }
        }

        /// <summary>
        /// inserts the data into the portfolio value history table
        /// </summary>
        /// <param name="_rESXFile">file path</param>
        public void InsertInPortfolioValueHistory(string _rESXFile)
        {
            ExecuteQuery($"USE {Connections.xInstance.DATABASENAME};"
                , SQLConnection);

            string sQLQuery = @"INSERT INTO PortfolioValueHistory (USERID, TOTALVALUE) VALUES (@USERID, @TOTALVALUE) ON DUPLICATE KEY UPDATE USERID = @USERID, TOTALVALUE = @TOTALVALUE;";

            using (MySqlTransaction sQLTransaction = SQLConnection.BeginTransaction())
            using (MySqlCommand sQLCommand = new MySqlCommand(sQLQuery, SQLConnection, sQLTransaction))
            {
                try
                {
                    PortfolioValueHistory.PortfolioValueHistoryMap.ToList().ForEach(U =>
                    {
                        sQLCommand.Parameters.Clear();

                        sQLCommand.Parameters.AddWithValue("@USERID", U.Value.UserID);
                        sQLCommand.Parameters.AddWithValue("@TOTALVALUE", U.Value.TotalValue);

                        sQLCommand.ExecuteNonQuery();
                    });

                    sQLTransaction.Commit();
                }
                catch (Exception EX)
                {
                    sQLTransaction.Rollback();
                    Console.WriteLine($"{Import.Resources.Labels.DataImportFailed} {EX.Message}");
                }
            }
        }

        /// <summary>
        /// inserts the data into the user transaction table
        /// </summary>
        /// <param name="_rESXFile">file path</param>
        public void InsertInTransaction(string _rESXFile)
        {
            ExecuteQuery($"USE {Connections.xInstance.DATABASENAME};"
               , SQLConnection);

            string sQLQuery = @"INSERT INTO Transaction (USERID, STOCKID, TRANSACTIONTYPE, QUANTITY, PRICE) VALUES (@USERID, @STOCKID, @TRANSACTIONTYPE, @QUANTITY, @PRICE) ON DUPLICATE KEY UPDATE USERID = @USERID, STOCKID = @STOCKID, TRANSACTIONTYPE = @TRANSACTIONTYPE, QUANTITY = @QUANTITY, PRICE = @PRICE;";

            using (MySqlTransaction sQLTransaction = SQLConnection.BeginTransaction())
            using (MySqlCommand sQLCommand = new MySqlCommand(sQLQuery, SQLConnection, sQLTransaction))
            {
                try
                {
                    Transaction.TransactionMap.ToList().ForEach(U =>
                    {
                        sQLCommand.Parameters.Clear();

                        sQLCommand.Parameters.AddWithValue("@USERID", U.Value.UserID);
                        sQLCommand.Parameters.AddWithValue("@STOCKID", U.Value.StockID);
                        sQLCommand.Parameters.AddWithValue("@TRANSACTIONTYPE", U.Value.TransactionType);
                        sQLCommand.Parameters.AddWithValue("@QUANTITY", U.Value.Quantity);
                        sQLCommand.Parameters.AddWithValue("@PRICE", U.Value.Price);

                        sQLCommand.ExecuteNonQuery();
                    });

                    sQLTransaction.Commit();
                }
                catch (Exception EX)
                {
                    sQLTransaction.Rollback();
                    Console.WriteLine($"{Import.Resources.Labels.DataImportFailed} {EX.Message}");
                }
            }
        }

        /// <summary>
        /// inserts the data into the user portfolio table
        /// </summary>
        /// <param name="_rESXFile">file path</param>
        public void InsertInUserPortfolio(string _rESXFile)
        {
            ExecuteQuery($"USE {Connections.xInstance.DATABASENAME};"
               , SQLConnection);

            string sQLQuery = @"INSERT INTO UserPortfolio (USERID, STOCKID, QUANTITY) VALUES (@USERID, @STOCKID, @QUANTITY) ON DUPLICATE KEY UPDATE USERID = @USERID, STOCKID = @STOCKID, QUANTITY = @QUANTITY;";

            using (MySqlTransaction sQLTransaction = SQLConnection.BeginTransaction())
            using (MySqlCommand sQLCommand = new MySqlCommand(sQLQuery, SQLConnection, sQLTransaction))
            {
                try
                {
                    UserPortfolio.UserPortfolioMap.ToList().ForEach(U =>
                    {
                        sQLCommand.Parameters.Clear();

                        sQLCommand.Parameters.AddWithValue("@USERID", U.Value.UserID);
                        sQLCommand.Parameters.AddWithValue("@STOCKID", U.Value.StockID);
                        sQLCommand.Parameters.AddWithValue("@QUANTITY", U.Value.Quantity);

                        sQLCommand.ExecuteNonQuery();
                    });

                    sQLTransaction.Commit();
                }
                catch (Exception EX)
                {
                    sQLTransaction.Rollback();
                    Console.WriteLine($"{Import.Resources.Labels.DataImportFailed} {EX.Message}");
                }
            }
        }

        /// <summary>
        /// inserts the data into the user stpck table
        /// </summary>
        /// <param name="_rESXFile">file path</param>
        public void InsertInStock(string _rESXPath, List<string> _rESXFiles)
        {
            SQLInitializer.ExecuteQuery($"USE {Connections.xInstance.DATABASENAME};"
                , SQLConnection);

            StockDataService.ImportIntoStock(SQLConnection, Stock.StockPriceMap);
        }

    }
}
