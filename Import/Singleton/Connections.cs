using Newtonsoft.Json;

namespace Import.Singleton
{
    /// <summary>
    /// singleton class to ensure that only one instance of the class is created
    /// </summary>
    public class Connections : SingletonBase<Connections>
    {
        [JsonProperty("APIKEY")]
        public string APIKEY { get; set; }
        
        [JsonProperty("CONNECTIONSTRING")]
        public string CONNECTIONSTRING { get; set; }

        [JsonProperty("DATABASENAME")]
        public string DATABASENAME {  get; set; }
    }
}
