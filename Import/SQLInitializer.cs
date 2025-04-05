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
        }

        /// <summary>
        /// create database
        /// </summary>
        private void CreateDatabase()
        {
            ExecuteQuery($@"
                IF NOT EXISTS (SELECT NAME FROM SYS.DATABASES WHERE NAME = '{Connections.DATABASENAME}')
                    BEGIN
                        CREATE DATABASE {Connections.DATABASENAME};
                    END
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
                IF OBJECT_ID('USER', 'U') IS NULL
                    BEGIN
                        CREATE TABLE USER (
                            USERID INT PRIMARY KEY IDENTITY(1,1),           
                            USERNAME VARCHAR(255) NOT NULL,                  
                            EMAIL VARCHAR(255) NOT NULL,                    
                            PASSWORDHASH VARCHAR(255) NOT NULL,             
                            BALANCE DECIMAL(15, 2) DEFAULT 0.00             
                        );
                    END
                ");

            ExecuteQuery(@"
                IF OBJECT_ID('STOCK', 'U') IS NULL
                    BEGIN
                        CREATE TABLE STOCK (
                            STOCKID INT PRIMARY KEY IDENTITY(1,1),           
                            TICKERSYMBOL VARCHAR(10) NOT NULL UNIQUE,        
                            COMPANYNAME VARCHAR(255) NOT NULL,              
                            CURRENTPRICE DECIMAL(10, 2) NOT NULL,           
                            MARKETCAP DECIMAL(20, 2),                       
                            SECTOR VARCHAR(100),                             
                            CURRENCY VARCHAR(3),                             
                            LASTUPDATED TIMESTAMP DEFAULT CURRENT_TIMESTAMP   
                        );
                    END
            ");

            ExecuteQuery(@"
                IF OBJECT_ID('TRANSACTION', 'U') IS NULL
                    BEGIN
                        CREATE TABLE TRANSACTION (
                            TRANSACTIONID INT PRIMARY KEY IDENTITY(1,1),    
                            USERID INT NOT NULL,                              
                            STOCKID INT NOT NULL,                             
                            TRANSACTIONTYPE VARCHAR(4) CHECK (TRANSACTIONTYPE IN ('BUY', 'SELL')) NOT NULL,     
                            QUANTITY INT NOT NULL,                            
                            PRICE DECIMAL(10, 2) NOT NULL,                    
                            TRANSACTIONDATE TIMESTAMP DEFAULT CURRENT_TIMESTAMP, 
                            FOREIGN KEY (USERID) REFERENCES USER(USERID),    
                            FOREIGN KEY (STOCKID) REFERENCES STOCK(STOCKID)  
                        );
                    END
            ");

            ExecuteQuery(@"
                IF OBJECT_ID('USERPORTFOLIO', 'U') IS NULL
                    BEGIN
                        CREATE TABLE USERPORTFOLIO (
                            PORTFOLIOID INT PRIMARY KEY IDENTITY(1,1),       
                            USERID INT NOT NULL,                              
                            STOCKID INT NOT NULL,                             
                            QUANTITY INT NOT NULL,                            
                            FOREIGN KEY (USERID) REFERENCES USER(USERID),    
                            FOREIGN KEY (STOCKID) REFERENCES STOCK(STOCKID), 
                            UNIQUE(USERID, STOCKID)                          
                        );
                    END
            ");

            ExecuteQuery(@"
                IF OBJECT_ID('PORTFOLIOVALUEHISTORY', 'U') IS NULL
                    BEGIN
                        CREATE TABLE PORTFOLIOVALUEHISTORY (
                            HISTORYID INT PRIMARY KEY IDENTITY(1,1),        
                            USERID INT NOT NULL,                              
                            TOTALVALUE DECIMAL(15, 2) NOT NULL,               
                            DATE TIMESTAMP DEFAULT CURRENT_TIMESTAMP,         
                            FOREIGN KEY (USERID) REFERENCES USER(USERID)     
                        );
                    END
            ");
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
