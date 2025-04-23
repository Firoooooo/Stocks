using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StocksAPI.Data;
using StocksAPI.Models;

namespace StocksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioValueHistoryController : ControllerBase
    {
        private readonly StocksDBContext sTOCKDontext;


        /// <summary>
        /// accepts content injected via the dependency injection in order to store it in a variable
        /// </summary>
        /// <param name="_sTOCKDontext">the context of dependency injection</param>
        public PortfolioValueHistoryController(StocksDBContext _sTOCKDontext)
        {
            sTOCKDontext = _sTOCKDontext;
        }

        /// <summary>
        /// retrieves all portfolio values historys from the database
        /// </summary>
        /// <returns>Task</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllPortfolioValueHistorys()
        {
            return await sTOCKDontext.PortfolioValueHistory.ToListAsync() is List<Models.PortfolioValueHistory> pORT ? Ok(pORT) : NotFound();
        }

        /// <summary>
        /// retrieves a specific portfolio value history from the database
        /// </summary>
        /// <param name="_pORTID">unique identifier for each portfolio value history</param>
        /// <returns>Task</returns>
        [HttpGet("{_pORTID}")]
        public async Task<IActionResult> GetPortfolioValueHistory(int _pORTID)
        {
            var pORT = await sTOCKDontext.PortfolioValueHistory.FindAsync(_pORTID);
            if (pORT == null)
                return NotFound();

            return Ok(pORT);
        }

        /// <summary>
        /// creates a new portfolio value history in the database
        /// </summary>
        /// <param name="_pORT">portfolio value history which is getting inserted</param>
        /// <returns>Task</returns>
        [HttpPost]
        public async Task<ActionResult<Stock>> CreatePortfolioValueHistory(PortfolioValueHistory _pORT)
        {
            sTOCKDontext.PortfolioValueHistory.Add(_pORT);
            await sTOCKDontext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPortfolioValueHistory), new { HISTORYID = _pORT.HistoryID }, _pORT);
        }

        /// <summary>
        /// updates the portfolio value history in the database
        /// </summary>
        /// <param name="_pORTID">unique identifier for each portfolio value history</param>
        /// <param name="_pORT">portfolio value history which is getting inserted</param>
        /// <returns>Task</returns>
        [HttpPut("{_pORTID}")]
        public async Task<IActionResult> UpdatePortfolioValueHistory(int _pORTID, PortfolioValueHistory _pORT)
        {
            if (_pORTID != _pORT.HistoryID)
                return BadRequest();

            sTOCKDontext.Entry(_pORT).State = EntityState.Modified;
            await sTOCKDontext.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// deletes the portfolio value history from the database
        /// </summary>
        /// <param name="_pORTID">unique identifier for each portfolio value history</param>
        /// <returns>Task</returns>
        [HttpDelete("{_pORTID}")]
        public async Task<IActionResult> DeletePortfolioValueHistory(int _pORTID)
        {
            var pORT = await sTOCKDontext.PortfolioValueHistory.FindAsync(_pORTID);
            if (pORT == null)
                return NotFound();

            sTOCKDontext.PortfolioValueHistory.Remove(pORT);
            await sTOCKDontext.SaveChangesAsync();

            return NoContent();
        }
    }
}
