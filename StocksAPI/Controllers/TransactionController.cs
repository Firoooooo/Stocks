using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StocksAPI.Data;

namespace StocksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly StocksDBContext sTOCKDontext;


        /// <summary>
        /// accepts content injected via the dependency injection in order to store it in a variable
        /// </summary>
        /// <param name="_sTOCKDontext">the context of dependency injection</param>
        public TransactionController(StocksDBContext _sTOCKDontext)
        {
            sTOCKDontext = _sTOCKDontext;
        }

        /// <summary>
        /// retrieves all transactions from the database
        /// </summary>
        /// <returns>Task</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            return await sTOCKDontext.Transaction.ToListAsync() is List<Models.Transaction> tRANS ? Ok(tRANS) : NotFound();
        }

        /// <summary>
        /// retrieves a specific transaction from the database
        /// </summary>
        /// <param name="_tRANSID">unique identifier for each transaction</param>
        /// <returns>Task</returns>
        [HttpGet("{_tRANSID}")]
        public async Task<IActionResult> GetTransaction(int _tRANSID)
        {
            var tRANS = await sTOCKDontext.Transaction.FindAsync(_tRANSID);
            if (tRANS == null)
                return NotFound();

            return Ok(tRANS);
        }

        /// <summary>
        /// creates a new transaction in the database
        /// </summary>
        /// <param name="_tRANS">transaction which is getting inserted</param>
        /// <returns>Task</returns>
        [HttpPost]
        public async Task<ActionResult<StocksAPI.Models.Transaction>> CreateTransaction(StocksAPI.Models.Transaction _tRANS)
        {
            sTOCKDontext.Transaction.Add(_tRANS);
            await sTOCKDontext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaction), new { TRANSID = _tRANS.TransactionID}, _tRANS);
        }

        /// <summary>
        /// updates the transaction in the database
        /// </summary>
        /// <param name="_tRANSID">unique identifier for each transaction</param>
        /// <param name="_tRANS">transaction which is getting inserted</param>
        /// <returns>Task</returns>
        [HttpPut("{_tRANSID}")]
        public async Task<IActionResult> UpdateTransaction(int _tRANSID, StocksAPI.Models.Transaction _tRANS)
        {
            if (_tRANSID != _tRANS.TransactionID)
                return BadRequest();

            sTOCKDontext.Entry(_tRANS).State = EntityState.Modified;
            await sTOCKDontext.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// deletes the transaction from the database
        /// </summary>
        /// <param name="_tRANSID">unique identifier for each transaction</param>
        /// <returns>Task</returns>
        [HttpDelete("{_tRANSID}")]
        public async Task<IActionResult> DeleteTransaction(int _tRANSID)
        {
            var tRANS = await sTOCKDontext.Transaction.FindAsync(_tRANSID);
            if (tRANS == null)
                return NotFound();

            sTOCKDontext.Transaction.Remove(tRANS);
            await sTOCKDontext.SaveChangesAsync();

            return NoContent();
        }
    }
}
