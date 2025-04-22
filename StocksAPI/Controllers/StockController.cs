using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StocksAPI.Data;
using StocksAPI.Models;

namespace StocksAPI.Controllers
{
    /// <summary>
    /// controller for managing stock data in the stocks database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly StocksDBContext sTOCKDontext;


        /// <summary>
        /// accepts content injected via the dependency injection in order to store it in a variable
        /// </summary>
        /// <param name="_sTOCKDontext">the context of dependency injection</param>
        public StockController(StocksDBContext _sTOCKDontext)
        {
            sTOCKDontext = _sTOCKDontext;
        }

        /// <summary>
        /// retrieves all stocks from the database
        /// </summary>
        /// <returns>Task</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllStocks()
        {
            return await sTOCKDontext.Stock.ToListAsync() is List<Models.Stock> sTOCK ? Ok(sTOCK) : NotFound();
        }

        /// <summary>
        /// retrieves a specific stock from the database
        /// </summary>
        /// <param name="_sTOCKID">unique identifier for each stock</param>
        /// <returns>Task</returns>
        [HttpGet("{_sTOCKID}")]
        public async Task<IActionResult> GetStock(int _sTOCKID)
        {
            var sTOCK = await sTOCKDontext.Stock.FindAsync(_sTOCKID);
            if (sTOCK == null)
                return NotFound();

            return Ok(sTOCK);
        }

        /// <summary>
        /// creates a new stock in the database
        /// </summary>
        /// <param name="_sTOCK">stock which is getting inserted</param>
        /// <returns>Task</returns>
        [HttpPost]
        public async Task<ActionResult<Stock>> CreateStock(Stock _sTOCK)
        {
            sTOCKDontext.Stock.Add(_sTOCK);
            await sTOCKDontext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStock), new { STOCKID = _sTOCK.StockID }, _sTOCK);
        }

        /// <summary>
        /// updates the stock in the database
        /// </summary>
        /// <param name="_sTOCKID">unique identifier for each stock</param>
        /// <param name="_sTOCK">stock which is getting inserted</param>
        /// <returns>Task</returns>
        [HttpPut("{_sTOCKID}")]
        public async Task<IActionResult> UpdateStock(int _sTOCKID, Stock _sTOCK)
        {
            if (_sTOCKID != _sTOCK.StockID)
                return BadRequest();

            sTOCKDontext.Entry(_sTOCK).State = EntityState.Modified;
            await sTOCKDontext.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// deletes the stock from the database
        /// </summary>
        /// <param name="_sTOCKID">unique identifier for each stock</param>
        /// <returns>Task</returns>
        [HttpDelete("{_sTOCKID}")]
        public async Task<IActionResult> DeleteStock(int _sTOCKID)
        {
            var sTOCK = await sTOCKDontext.Stock.FindAsync(_sTOCKID);
            if (sTOCK == null)
                return NotFound();

            sTOCKDontext.Stock.Remove(sTOCK);
            await sTOCKDontext.SaveChangesAsync();

            return NoContent();
        }
    }
}
