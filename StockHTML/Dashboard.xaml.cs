using Import.Singleton;
using Newtonsoft.Json;
using StockHTML.SQL;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows;

namespace StockHTML
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public string EMail { get; set; }
        public Connections Connections { get; set; }
        public DataTable DataTable { get; set; }


        /// <summary>
        /// constructor for the dashboard window
        /// </summary>
        /// <param name="_eMAIL">email address for the specified user</param>
        public Dashboard(string _eMAIL)
        {
            InitializeComponent();
            EMail = _eMAIL;
            InitializeUserWindow();
        }

        /// <summary>
        /// the relevant information is loaded on the basis of the address provided
        /// </summary>
        public void InitializeUserWindow()
        {
            Connections = Connections.xInstance;

            using (Stream rESXStream = Assembly.Load("Import").GetManifestResourceStream("Import.Configs.Config.json"))
            using (StreamReader rESXStreamReader = new StreamReader(rESXStream))
            {
                string rESXCOntent = rESXStreamReader.ReadToEnd();
                JsonConvert.PopulateObject(rESXCOntent, Connections);
            }

            SQLHandler sQLHandler = new SQLHandler(Connections, EMail);
            DataTable = sQLHandler.RetrieveUsersAsDataTable();
        }
    }
}
