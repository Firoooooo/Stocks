﻿namespace Import.Model
{
    /// <summary>
    /// data transfer object for stock prices
    /// </summary>
    public class DTOStockPrice
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }
    }
}
