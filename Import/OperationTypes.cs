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
        SchemaBuilder
    }
}
