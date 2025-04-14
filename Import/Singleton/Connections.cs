using Newtonsoft.Json;

namespace Import.Singleton
{
    /// <summary>
    /// singleton class to ensure that only one instance of the class is created
    /// </summary>
    public class Connections
    {
        [JsonProperty("APIKEY")]
        public string APIKEY { get; set; }
        
        [JsonProperty("CONNECTIONSTRING")]
        public string CONNECTIONSTRING { get; set; }

        [JsonProperty("DATABASENAME")]
        public string DATABASENAME {  get; set; }
        private static Connections Instance { get; set; }


        /// <summary>
        /// if an instance of the class exixts, it returns the instance, otherwise it creates a new instance
        /// </summary>
        /// <returns><Connections/returns>
        public static Connections GetInstance()
        {
            if (Instance == null)
                return new Connections();

            return Instance;
        }
    }
}
