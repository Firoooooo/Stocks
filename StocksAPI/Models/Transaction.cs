namespace StocksAPI.Models
{
    /// <summary>
    /// model class representing a transaction entity in the database
    /// </summary>
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int UserID { get; set; }
        public int StockID { get; set; }
        public string TransactionType { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime TransactionDate { get; set; }

        public User User { get; set; }
        public Stock Stock { get; set; }
    }

}
