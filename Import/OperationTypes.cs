namespace Import
{
    /// <summary>
    /// runnable classes that can be executed
    /// </summary>
    public enum OperationTypes
    {
        /// <summary>
        /// import the stacks to the database
        /// </summary>
        DBImportStacks,

        /// <summary>
        /// clear the stocks from the database
        /// </summary>
        ClearStocks,

        /// <summary>
        /// import the schema to the database
        /// </summary>
        SchemaBuilder,

        /// <summary>
        /// visualizes all upload possibilities
        /// </summary>
        UploadHandler,

        /// <summary>
        /// import the stocks to the database via api
        /// </summary>
        StockAPI,

        /// <summary>
        /// import the stocks to the database
        /// </summary>
        Stock,

        /// <summary>
        /// import the portfolio to the database
        /// </summary>
        PortfolioValueHistory,

        /// <summary>
        /// import the portfolio to the database
        /// </summary>
        Transaction,

        /// <summary>
        /// import the portfolio to the database
        /// </summary>
        User,

        /// <summary>
        /// import the portfolio to the database
        /// </summary>
        UserPortfolio
    }
}
