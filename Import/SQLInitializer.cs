using DocumentFormat.OpenXml.Spreadsheet;
using MySql.Data.MySqlClient;
using CON = Import.Context;

namespace Import
{
    public class SQLInitializer
    {
        public CON.Connections Connections {  get; set; }
        public MySqlConnection SQLConnection { get; set; }

        /// <summary>
        /// constuctor
        /// </summary>
        public SQLInitializer(CON.Connections _cON) 
        {
            Connections = _cON;
            SQLConnection = new MySqlConnection(Connections.CONNECTIONSTRING);
            SQLConnection.Open();
        }

        /// <summary>
        /// create database
        /// </summary>
        private void CreateDatabase()
        {
            ExecuteQuery($@"
                CREATE DATABASE IF NOT EXISTS {Connections.DATABASENAME};
                ");
        }

        /// <summary>
        /// create tables in the corresponding database
        /// </summary>
        public void Initialize()
        {
            CreateDatabase();
            ExecuteQuery($"USE {Connections.DATABASENAME};");

            ExecuteQuery(@"
                CREATE TABLE IF NOT EXISTS USER (
                    USERID INT AUTO_INCREMENT PRIMARY KEY,           
                    USERNAME VARCHAR(255) NOT NULL,                  
                    EMAIL VARCHAR(255) NOT NULL,                    
                    PASSWORDHASH VARCHAR(255) NOT NULL,             
                    BALANCE DECIMAL(15, 2) DEFAULT 0.00              
                );");

            ExecuteQuery(@"
                CREATE TABLE IF NOT EXISTS Stock (
                    STOCKID INT AUTO_INCREMENT PRIMARY KEY,           
                    TICKERSYMBOL VARCHAR(10) NOT NULL UNIQUE,        
                    COMPANYNAME VARCHAR(255) NOT NULL,              
                    CURRENTPRICE DECIMAL(10,2) NOT NULL,           
                    MARKETCAP DECIMAL(20,2),                       
                    SECTOR VARCHAR(100),                             
                    CURRENCY VARCHAR(3),                             
                    LASTUPDATED TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
                );");

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
                );");

            ExecuteQuery(@"
                CREATE TABLE IF NOT EXISTS USERPORTFOLIO (
                    PORTFOLIOID INT AUTO_INCREMENT PRIMARY KEY,       
                    USERID INT NOT NULL,                              
                    STOCKID INT NOT NULL,                             
                    QUANTITY INT NOT NULL,                            
                    FOREIGN KEY (USERID) REFERENCES `USER`(USERID),    
                    FOREIGN KEY (STOCKID) REFERENCES `STOCK`(STOCKID), 
                    UNIQUE(USERID, STOCKID)
                );");

            ExecuteQuery(@"
                CREATE TABLE IF NOT EXISTS PORTFOLIOVALUEHISTORY (
                    HISTORYID INT AUTO_INCREMENT PRIMARY KEY,        
                    USERID INT NOT NULL,                              
                    TOTALVALUE DECIMAL(15,2) NOT NULL,               
                    DATE TIMESTAMP DEFAULT CURRENT_TIMESTAMP,         
                    FOREIGN KEY (USERID) REFERENCES `USER`(USERID)     
                );");
        }

        /// <summary>
        /// executes the query
        /// </summary>
        /// <param name="_sQLQuery">query to execute</param>
        public void ExecuteQuery(string _sQLQuery)
        {
            using (MySqlCommand sQLCom = new MySqlCommand(_sQLQuery, SQLConnection))
            {
                sQLCom.ExecuteNonQuery();
            }
        }
    }
}
