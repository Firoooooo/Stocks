namespace StocksAPI.Models
{
    /// <summary>
    /// model class representing a user portfolio entity in the database
    /// </summary>
    public class UserPortfolio
    {
        public int PortfolioID { get; set; }
        public int UserID { get; set; }
        public int StockID { get; set; }
        public int Quantity { get; set; }

        public User User { get; set; }
        public Stock Stock { get; set; }
    }

}
