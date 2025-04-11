using Import.Context;
using MySql.Data.MySqlClient;
using System.Data;

namespace Import
{
    public class SQLInitializer
    {
        public Connections CON { get; set; }
        public MySqlConnection SQLConnection { get; set; }


        /// <summary>
        /// constructor that receives the context and opens the connection
        /// </summary>
        /// <param name="_cON">context class</param>
        public SQLInitializer(Connections _cON)
        {
            CON = _cON;
            SQLConnection = new MySqlConnection(CON.CONNECTIONSTRING);
            SQLConnection.Open();
        }

        /// <summary>
        /// create database
        /// </summary>
        private void CreateDatabase()
        {
            ExecuteQuery($@"
                CREATE DATABASE IF NOT EXISTS {CON.DATABASENAME};
                ", SQLConnection);
        }

        /// <summary>
        /// create tables in the corresponding database
        /// </summary>
        public void Initialize()
        {
            DataTable sQLDataTable;

            CreateDatabase();
            ExecuteQuery($"USE {CON.DATABASENAME};"
                , SQLConnection);

            ExecuteQuery(@"
                CREATE TABLE IF NOT EXISTS USER (
                    USERID INT AUTO_INCREMENT PRIMARY KEY,           
                    USERNAME VARCHAR(255) NOT NULL,                  
                    EMAIL VARCHAR(255) NOT NULL,                    
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
            ExecuteQuery($"USE {CON.DATABASENAME};"
                , SQLConnection);

            string sQLSecure = GetSecureFilePriv(_rESXFile);
            string sQLDestination = Path.Combine(sQLSecure, Path.GetFileName(_rESXFile));
            File.Copy(_rESXFile, sQLDestination, true);
            _rESXFile = sQLDestination;

            if (Path.GetExtension(_rESXFile).ToUpper() == ".CSV")
                ExecuteQuery($"LOAD DATA INFILE '{_rESXFile.Replace("\\", "/")}' INTO TABLE User FIELDS TERMINATED BY ',' LINES TERMINATED BY '\r\n' (USERNAME, EMAIL, PASSWORDHASH, BALANCE);", SQLConnection);
            if (Path.GetExtension(_rESXFile).ToUpper() == ".TXT")
                ExecuteQuery($"LOAD DATA INFILE '{_rESXFile.Replace("\\", "/")}' INTO TABLE User FIELDS TERMINATED BY '\t' LINES TERMINATED BY '\r\n' (USERNAME, EMAIL, PASSWORDHASH, BALANCE);", SQLConnection);

            File.Delete(_rESXFile);
        }

        /// <summary>
        /// inserts the data into the portfolio value history table
        /// </summary>
        /// <param name="_rESXFile"></param>
        public void InsertInPortfolioValueHistory(string _rESXFile)
        {
            ExecuteQuery($"USE {CON.DATABASENAME};"
                , SQLConnection);

            string sQLSecure = GetSecureFilePriv(_rESXFile);
            string sQLDestination = Path.Combine(sQLSecure, Path.GetFileName(_rESXFile));
            File.Copy(_rESXFile, sQLDestination, true);
            _rESXFile = sQLDestination;

            if (Path.GetExtension(_rESXFile).ToUpper() == ".CSV")
                ExecuteQuery($"LOAD DATA INFILE '{_rESXFile.Replace("\\", "/")}' INTO TABLE PortfolioValueHistory FIELDS TERMINATED BY ',' LINES TERMINATED BY '\r\n' (USERID, TOTALVALUE);", SQLConnection);
            if (Path.GetExtension(_rESXFile).ToUpper() == ".TXT")
                ExecuteQuery($"LOAD DATA INFILE '{_rESXFile.Replace("\\", "/")}' INTO TABLE PortfolioValueHistory FIELDS TERMINATED BY '\t' LINES TERMINATED BY '\r\n' (USERID, TOTALVALUE);", SQLConnection);

            File.Delete(_rESXFile);
        }

        /// <summary>
        /// inserts the data into the user transaction table
        /// </summary>
        /// <param name="_rESXFile"></param>
        public void InsertInTransaction(string _rESXFile)
        {
            ExecuteQuery($"USE {CON.DATABASENAME};"
               , SQLConnection);

            string sQLSecure = GetSecureFilePriv(_rESXFile);
            string sQLDestination = Path.Combine(sQLSecure, Path.GetFileName(_rESXFile));
            File.Copy(_rESXFile, sQLDestination, true);
            _rESXFile = sQLDestination;

            if (Path.GetExtension(_rESXFile).ToUpper() == ".CSV")
                ExecuteQuery($"LOAD DATA INFILE '{_rESXFile.Replace("\\", "/")}' INTO TABLE Transaction FIELDS TERMINATED BY ',' LINES TERMINATED BY '\r\n' (USERID, STOCKID, TRANSACTIONTYPE, QUANTITY, PRICE);", SQLConnection);
            if (Path.GetExtension(_rESXFile).ToUpper() == ".TXT")
                ExecuteQuery($"LOAD DATA INFILE '{_rESXFile.Replace("\\", "/")}' INTO TABLE Transaction FIELDS TERMINATED BY '\t' LINES TERMINATED BY '\r\n' (USERID, STOCKID, TRANSACTIONTYPE, QUANTITY, PRICE);", SQLConnection);

            File.Delete(_rESXFile);
        }

        /// <summary>
        /// inserts the data into the user portfolio table
        /// </summary>
        /// <param name="_rESXFile"></param>
        public void InsertInUserPortfolio(string _rESXFile)
        {
            ExecuteQuery($"USE {CON.DATABASENAME};"
               , SQLConnection);

            string sQLSecure = GetSecureFilePriv(_rESXFile);
            string sQLDestination = Path.Combine(sQLSecure, Path.GetFileName(_rESXFile));
            File.Copy(_rESXFile, sQLDestination, true);
            _rESXFile = sQLDestination;

            if (Path.GetExtension(_rESXFile).ToUpper() == ".CSV")
                ExecuteQuery($"LOAD DATA INFILE '{_rESXFile.Replace("\\", "/")}' INTO TABLE UserPortfolio FIELDS TERMINATED BY ',' LINES TERMINATED BY '\r\n' (USERID, STOCKID, QUANTITY);", SQLConnection);
            if (Path.GetExtension(_rESXFile).ToUpper() == ".TXT")
                ExecuteQuery($"LOAD DATA INFILE '{_rESXFile.Replace("\\", "/")}' INTO TABLE UserPortfolio FIELDS TERMINATED BY '\t' LINES TERMINATED BY '\r\n' (USERID, STOCKID, QUANTITY);", SQLConnection);

            File.Delete(_rESXFile);
        }
    }
}
