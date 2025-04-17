namespace Import.Model
{
    /// <summary>
    /// data transfer object for the portfolio
    /// </summary>
    public class DTOPortfolio
    {
        public int UserID { get; set; } 
        public int StockID { get; set; }
        public int Quantity { get; set; }
    }
}
