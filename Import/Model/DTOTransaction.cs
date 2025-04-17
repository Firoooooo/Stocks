namespace Import.Model
{
    /// <summary>
    /// data transfer object for transactions
    /// </summary>
    public class DTOTransaction
    {
        public int UserID { get; set; }
        public int StockID { get; set; }
        public string TransactionType { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
