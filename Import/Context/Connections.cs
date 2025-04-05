namespace Import.Context
{
    public class Connections
    {
        public string APIKEY { get; set; }
        public string CONNECTIONSTRING { get; set; }

        public string DATABASENAME {  get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public Connections(
            string _APIKEY,
            string _CONNECTIONSTRING,
            string _DATABASENAME) 
        {
            APIKEY = _APIKEY;
            CONNECTIONSTRING = _CONNECTIONSTRING;
            DATABASENAME = _DATABASENAME;
        }
    }
}
