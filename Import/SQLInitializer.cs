using Import.Context;
using MySql.Data.MySqlClient;

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
            CreateDatabase();
            ExecuteQuery($"USE {CON.DATABASENAME};", SQLConnection);

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
        /// loads corresponding data into the tables
        /// </summary>
        public void LoadDataIntoDatabase()
        {

        }

        /// <summary>
        /// executes the query
        /// </summary>
        /// <param name="_sQLQuery">query to execute</param>
        public static void ExecuteQuery(string _sQLQuery, MySqlConnection _sQLCon)
        {
            using (MySqlCommand sQLCom = new MySqlCommand(_sQLQuery, _sQLCon))
            {
                sQLCom.ExecuteNonQuery();
            }
        }
    }
}
