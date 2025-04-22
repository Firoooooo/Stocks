namespace StocksAPI.Models
{
    /// <summary>
    /// model class representing a stock entity in the database
    /// </summary>
    public class Stock
    {
        public int StockID { get; set; }
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }
        public DateTime LastUpdated { get; set; }

        public ICollection<UserPortfolio> UserPortfolios { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }

}
