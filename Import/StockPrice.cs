namespace Import
{
    /// <summary>
    /// stock informations
    /// </summary>
    public class StockPrice
    {
        /// <summary>
        /// ticker symbol of the stock
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// date of the data
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// opening price for the stock
        /// </summary>
        public decimal Open { get; set; }

        /// <summary>
        /// highest price for the stock on this day
        /// </summary>
        public decimal High { get; set; }

        /// <summary>
        /// lowest price of the stock this day
        /// </summary>
        public decimal Low { get; set; }

        /// <summary>
        /// closing price of the stock on this day 
        /// </summary>
        public decimal Close { get; set; }

        /// <summary>
        /// number of shares or units that have been traded on this day
        /// </summary>
        public long Volume { get; set; }
    }
}
