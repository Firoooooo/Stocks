using Newtonsoft.Json;

namespace Import.Context
{
    /// <summary>
    /// class that contains the connection information for the database and the api key
    /// </summary>
    public class Connections
    {
        [JsonProperty("APIKEY")]
        public string APIKEY { get; set; }
        
        [JsonProperty("CONNECTIONSTRING")]
        public string CONNECTIONSTRING { get; set; }

        [JsonProperty("DATABASENAME")]
        public string DATABASENAME {  get; set; }
    }
}
