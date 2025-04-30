namespace Import.Model
{
    /// <summary>
    /// data transfer object for users
    /// </summary>
    public class DTOUser
    {
        public string Username { get; set; }
        public string EMail { get; set; }
        public string Passowrd { get; set; }
        public string PasswordHash { get; set; }
        public decimal Balance { get; set; }
    }
}
