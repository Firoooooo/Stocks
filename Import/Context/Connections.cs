using Newtonsoft.Json;

namespace Import.Context
{
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
