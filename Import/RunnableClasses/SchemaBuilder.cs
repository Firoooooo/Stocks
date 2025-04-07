using CON = Import.Context;
using MySql.Data.MySqlClient;
using Import.Context;

namespace Import.RunnableClasses
{
    [RunnableClassAttribute(1, "Initialisierung der Datenbankstruktur einschließlich der Tabellen und Referenzen")]
    public class SchemaBuilder : RunnableClassBase
    {
        public CON.Connections CON { get; set; }
        public MySqlConnection SQLConnection { get; set; }

        /// <summary>
        /// base call
        /// </summary>
        /// <param name="_cON">connection context</param>
        public SchemaBuilder(Connections _cON) 
            : base(_cON)
        {
            CON = _cON;
            SQLConnection = new MySqlConnection(CON.CONNECTIONSTRING);
            SQLConnection.Open();
        }

        /// <summary>
        /// run method of the runnable classes
        /// </summary>
        public override void Run()
        {
            using (MySqlConnection sQLCon = new MySqlConnection(CON.CONNECTIONSTRING))
            {
                sQLCon.Open();

                Initialize();
            }
        }

        /// <summary>
        /// create database
        /// </summary>
        private void CreateDatabase()
        {
            ExecuteQuery($@"
                CREATE DATABASE IF NOT EXISTS {CON.DATABASENAME};
                ");
        }

        /// <summary>
        /// create tables in the corresponding database
        /// </summary>
        public void Initialize()
        {
            CreateDatabase();
            ExecuteQuery($"USE {CON.DATABASENAME};");

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
        /// loads corresponding data into the tables
        /// </summary>
        public void LoadDataIntoDatabase()
        {
            
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
