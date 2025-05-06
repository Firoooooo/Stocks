namespace StocksAPI.Models
{
    /// <summary>
    /// model class representing a user entity in the database
    /// </summary>
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public decimal Balance { get; set; }

        public ICollection<UserPortfolio> UserPortfolios { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<PortfolioValueHistory> PortfolioValueHistories { get; set; }
    }

}
