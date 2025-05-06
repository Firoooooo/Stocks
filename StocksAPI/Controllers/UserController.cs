using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StocksAPI.Data;
using StocksAPI.Models;

namespace StocksAPI.Controllers
{
    /// <summary>
    /// controller for managing user data in the stocks database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly StocksDBContext sTOCKDontext;


        /// <summary>
        /// accepts content injected via the dependency injection in order to store it in a variable
        /// </summary>
        /// <param name="_sTOCKDontext">the context of dependency injection</param>
        public UserController(StocksDBContext _sTOCKDontext)
        {
            sTOCKDontext = _sTOCKDontext;
        }

        /// <summary>
        /// retrieves all users from the database
        /// </summary>
        /// <returns>Task</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return await sTOCKDontext.User.ToListAsync() is List<Models.User> uSER ? Ok(uSER) : NotFound();
        }

        /// <summary>
        /// retrieves a specific user from the database
        /// </summary>
        /// <param name="_uSERN">unique identifier for each user</param>
        /// <returns>Task</returns>
        [HttpGet("{_uSERN}")]
        public async Task<IActionResult> GetUser(string _uSERN)
        {
            var uSER = await sTOCKDontext.User.FirstOrDefaultAsync(E => E.Email == _uSERN);
            if (uSER == null)
                return NotFound();

            return Ok(uSER);
        }

        /// <summary>
        /// creates a new user in the database
        /// </summary>
        /// <param name="_uSER">user which is getting inserted</param>
        /// <returns>Task</returns>
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User _uSER)
        {
            sTOCKDontext.User.Add(_uSER);
            await sTOCKDontext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { USERID = _uSER.UserID }, _uSER);
        }

        /// <summary>
        /// updates the user in the database
        /// </summary>
        /// <param name="_uSERN">unique identifier for each user</param>
        /// <param name="_uSER">user which is getting inserted</param>
        /// <returns>Task</returns>
        [HttpPut("{_uSERN}")]
        public async Task<IActionResult> UpdateUser(string _uSERN, User _uSER)
        {
            if (_uSERN != _uSER.Email)
                return BadRequest();

            sTOCKDontext.Entry(_uSER).State = EntityState.Modified;
            await sTOCKDontext.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// deletes the user from the database
        /// </summary>
        /// <param name="_uSERN">unique identifier for each user</param>
        /// <returns>Task</returns>
        [HttpDelete("{_uSERN}")]
        public async Task<IActionResult> DeleteUser(string _uSERN)
        {
            var uSER = await sTOCKDontext.User.FirstOrDefaultAsync(U => U.Email == _uSERN);
            if (uSER == null)
                return NotFound();

            sTOCKDontext.User.Remove(uSER);
            await sTOCKDontext.SaveChangesAsync();

            return NoContent();
        }
    }
}
