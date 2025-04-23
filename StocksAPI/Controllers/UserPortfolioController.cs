using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StocksAPI.Data;
using StocksAPI.Models;

namespace StocksAPI.Controllers
{
    /// <summary>
    /// controller for managing user portfolio data in the stocks database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserPortfolioController : ControllerBase
    {
        private readonly StocksDBContext sTOCKDontext;


        /// <summary>
        /// accepts content injected via the dependency injection in order to store it in a variable
        /// </summary>
        /// <param name="_sTOCKDontext">the context of dependency injection</param>
        public UserPortfolioController(StocksDBContext _sTOCKDontext)
        {
            sTOCKDontext = _sTOCKDontext;
        }

        /// <summary>
        /// retrieves all user portfolio from the database
        /// </summary>
        /// <returns>Task</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUserPortfolios()
        {
            return await sTOCKDontext.UserPortfolio.ToListAsync() is List<Models.UserPortfolio> uSERPortfolio ? Ok(uSERPortfolio) : NotFound();
        }

        /// <summary>
        /// retrieves a specific user portfolio from the database
        /// </summary>
        /// <param name="_uSERPortfolioID">unique identifier for each user portfolio</param>
        /// <returns>Task</returns>
        [HttpGet("{_uSERPortfolioID}")]
        public async Task<IActionResult> GetUserPortfolio(int _uSERPortfolioID)
        {
            var uSERPortfolio = await sTOCKDontext.UserPortfolio.FindAsync(_uSERPortfolioID);
            if (uSERPortfolio == null)
                return NotFound();

            return Ok(uSERPortfolio);
        }

        /// <summary>
        /// creates a new user portfolio in the database
        /// </summary>
        /// <param name="_uSERPortfolio">user portfolio which is getting inserted</param>
        /// <returns>Task</returns>
        [HttpPost]
        public async Task<ActionResult<UserPortfolio>> CreateUserPortfolio(UserPortfolio _uSERPortfolio)
        {
            sTOCKDontext.UserPortfolio.Add(_uSERPortfolio);
            await sTOCKDontext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserPortfolio), new { USERPORTFOLIOID = _uSERPortfolio.UserID }, _uSERPortfolio);
        }

        /// <summary>
        /// updates the user portfolio in the database
        /// </summary>
        /// <param name="_uSERPortfolioID">unique identifier for each user portfolio</param>
        /// <param name="_uSERPortfolio">user portfolio of the inserted</param>
        /// <returns>Task</returns>
        [HttpPut("{_uSERPortfolioID}")]
        public async Task<IActionResult> UpdateUserPortfolio(int _uSERPortfolioID, UserPortfolio _uSERPortfolio)
        {
            if (_uSERPortfolioID != _uSERPortfolio.StockID)
                return BadRequest();

            sTOCKDontext.Entry(_uSERPortfolio).State = EntityState.Modified;
            await sTOCKDontext.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// deletes the user portfolio from the database
        /// </summary>
        /// <param name="_uSERPortfolioID">unique identifier for each user portfolio</param>
        /// <returns>Task</returns>
        [HttpDelete("{_uSERPortfolioID}")]
        public async Task<IActionResult> DeleteUserPortfolio(int _uSERPortfolioID)
        {
            var uSERPortfolio = await sTOCKDontext.UserPortfolio.FindAsync(_uSERPortfolioID);
            if (uSERPortfolio == null)
                return NotFound();

            sTOCKDontext.UserPortfolio.Remove(uSERPortfolio);
            await sTOCKDontext.SaveChangesAsync();

            return NoContent();
        }
    }
}
