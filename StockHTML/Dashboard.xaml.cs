using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Import;
using Import.Singleton;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using StockHTML.SQL;

namespace StockHTML
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public string EMail { get; set; }
        public Connections Connections { get; set; }


        /// <summary>
        /// constructor for the dashboard window
        /// </summary>
        /// <param name="_eMAIL">email address for the specified user</param>
        public Dashboard(string _eMAIL)
        {
            InitializeComponent();
            EMail = _eMAIL;
            InitializeUserWindow();

            Connections cON = Connections.xInstance;

            using (Stream rESXFile = Assembly.Load("Import").GetManifestResourceStream("Import.Configs.Config.json"))
            {
                using (StreamReader rESXReader = new StreamReader(rESXFile))
                {
                    string rESXContent = rESXReader.ReadToEnd();

                    JsonConvert.PopulateObject(rESXContent, cON);
                }
            }

            Connections = cON;
        }

        /// <summary>
        /// the relevant information is loaded on the basis of the address provided
        /// </summary>
        public void InitializeUserWindow()
        {
            SQLHandler sQLHandler = new SQLHandler(Connections, EMail);


            SQLInitializer sQLInitializer = new SQLInitializer();
            MySqlConnection sQLConnection = new MySqlConnection(Connections.CONNECTIONSTRING);
            sQLConnection.Open();
            string sQLQuery = $"SELECT * FROM User WHERE Email = '{EMail}'";

            using (MySqlTransaction sQLTransaction = sQLConnection.BeginTransaction())
            using (MySqlCommand sQLCommand = new MySqlCommand(sQLQuery, sQLConnection))
            {
                try
                {


                    // ExecuteNonQuery gibt nicht das Ergebnis wieder - baue das also um 
                    sQLCommand.ExecuteNonQuery();
                    sQLTransaction.Commit();
                }
                catch (Exception EX)
                {
                    sQLTransaction.Rollback();
                    Console.WriteLine($"Error: {EX.Message}");
                }
            }
            
        }
    }

}
