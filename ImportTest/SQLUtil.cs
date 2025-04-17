using Import.Singleton;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Reflection;

namespace ImportTest
{
    /// <summary>
    /// class to check whether the individual data can be read out
    /// </summary>
    [TestClass]
    public class SQLUtil
    {
        /// <summary>
        /// test method to determine whether a connection to the database can be established via the connection string
        /// </summary>
        [TestMethod]
        public void CheckDatabaseConnection()
        {
            Connections cON = Connections.xInstance;

            using (Stream rESXStream = Assembly.Load("Import").GetManifestResourceStream("Import.Configs.Config.json"))
            using (StreamReader rESXStreamReader = new StreamReader(rESXStream))
            {
                string rESXCOntent = rESXStreamReader.ReadToEnd();
                JsonConvert.PopulateObject(rESXCOntent, cON);

                using (MySqlConnection sQLConnection = new MySqlConnection(cON.CONNECTIONSTRING))
                {
                    sQLConnection.Open();
                    Assert.IsTrue(sQLConnection.State == System.Data.ConnectionState.Open);
                }
            }
        }


    }
}
