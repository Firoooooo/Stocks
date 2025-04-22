namespace StocksAPI.Models
{
    /// <summary>
    /// model class representing a portfolio value history entity in the database
    /// </summary>
    public class PortfolioValueHistory
    {
        public int HistoryID { get; set; }
        public int UserID { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime Date { get; set; }

        public User User { get; set; }
    }

}
