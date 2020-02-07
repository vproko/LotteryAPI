using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lottery.DataModels.Models;
using Lottery.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;
        private readonly ITicketService _ticketService;
        private readonly IWinnerService _winnerService;

        public UserController(IUserService userService, ISessionService sessionService, ITicketService ticketService, IWinnerService winnerService)
        {
            _userService = userService;
            _sessionService = sessionService;
            _ticketService = ticketService;
            _winnerService = winnerService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterModel register)
        {
            if (ModelState.IsValid)
            {
                bool result = await _userService.UserRegistrationAsync(register, "User");

                if (result) return Ok("The user was registered successfully.");
            }

            return BadRequest("Something went wrong. Try again.");
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] LoginModel login)
        {
            if (ModelState.IsValid)
            {
                UserDTO user = await _userService.AuthenticateAsync(login);

                if (user != null) return Ok(user);
            }

            return BadRequest("Authentication has failed. Try again.");
        }

        [AllowAnonymous]
        [HttpGet("session/active")]
        public async Task<ActionResult> ActiveSession()
        {
            SessionDTO session = await _sessionService.IsThereActiveSessionAsync();

            if (session != null)
            {
                return Ok(session);
            }

            return BadRequest("There's no active session at the moment.");
        }

        [Authorize(Roles = "User")]
        [HttpPost("ticket")]
        public async Task<ActionResult> CreateTicket([FromBody] TicketDTO ticket)
        {
            if (ModelState.IsValid)
            {
                bool result = await _ticketService.CreateTicketAsync(ticket);

                if (result) return Ok("Your ticket is in the game. Good Luck!");
            }

            return BadRequest("Something went wrong, your ticket haven't been registered.");
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO result = await _userService.UpdateUserAsync(model);

                if(result != null)
                {
                    return Ok(result);
                }
            }

            return BadRequest("Something went wrong, try again");
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            if (userId != null)
            {
                bool result = await _userService.DeleteUserAsync(userId);

                if (result) return Ok("Your profile has been successfully deleted.");
            }

            return BadRequest("Something went wrong, try again");
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost("winners")]
        public async Task<IActionResult> GetAllWinners()
        {
            IEnumerable<WinnerDTO> winners = await _winnerService.GetAllWinnersAsync();

            if (winners.Count() > 0) return Ok(winners);

            return BadRequest("No winners in the last session.");
        }

        [AllowAnonymous]
        [HttpPost("check")]
        public async Task<IActionResult> CheckNumbers(string numbers)
        {
            if (String.IsNullOrEmpty(numbers)) return BadRequest("Something went wrong.");

            CheckModel winner = await Task.Run(() => _winnerService.CheckNumbersAsync(numbers));

            if (winner == null) return BadRequest("Sorry, no luck this time.");

            return Ok(winner);
        }

        [Authorize(Roles = "User")]
        [HttpPost("winner")]
        public async Task<IActionResult> GetWinnerById(Guid userId)
        {
            WinnerDTO winner = await _winnerService.GetWinnerByIdAsync(userId);

            if (winner != null) return Ok(winner);

            return BadRequest("Sorry, you haven't won anything. More luck next time.");
        }

        [AllowAnonymous]
        // GET api/user
        [HttpGet]
        public ActionResult<string> ApiStatus()
        {
            return "Lottery API is active.";
        }
    }
}