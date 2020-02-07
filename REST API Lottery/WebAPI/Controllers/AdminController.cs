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
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;
        private readonly IDrawService _drawService;
        private readonly IPrizeService _prizeService;

        public AdminController(IUserService userService, 
                               ISessionService sessionService, 
                               IDrawService drawService, 
                               IPrizeService prizeService)
        {
            _userService = userService;
            _sessionService = sessionService;
            _drawService = drawService;
            _prizeService = prizeService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("session")]
        public async Task<ActionResult> StartSession()
        {
            SessionDTO result = await _sessionService.CreateSessionAsync();

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest("There's already active session.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<ActionResult> GetAllUsers()
        {
            IEnumerable<UserDTO> users = await _userService.GetAllUsersAsync();

            if (users != null)
            {
                return Ok(users);
            }

            return BadRequest("There are no users at the moment.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("end")]
        public async Task<ActionResult> DeleteSession(Guid sessionId)
        {
            bool result = await _sessionService.DeleteSessionAsync(sessionId);

            if (result)
            {
                return Ok("The session has been deleted successfully.");
            }

            return BadRequest("Something went wrong.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("draw")]
        public async Task<ActionResult> Draw([FromBody] DrawDTO draw)
        {
            if (ModelState.IsValid)
            {
                bool result = await _drawService.CreateDrawAsync(draw);

                if (result) return Ok("Drawn lotto numbers has been published.");
            }

            return BadRequest("Either the numbers are already drawn, or something went wrong along the way.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAdmin([FromBody] RegisterModel register)
        {
            if (ModelState.IsValid)
            {
                bool result = await _userService.UserRegistrationAsync(register, "Admin");

                if (result) return Ok("The new administrator was registered successfully.");
            }

            return BadRequest("Something went wrong. Try again.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("prizes")]
        public async Task<ActionResult> GetAllPrizes()
        {
            IEnumerable<PrizeDTO> prizes = await _prizeService.GetAllPrizesAsync();

            if (prizes != null)
            {
                return Ok(prizes);
            }

            return BadRequest("There are no prizes.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("prize/create")]
        public async Task<ActionResult> CreatePrize([FromBody] PrizeDTO prize)
        {
            if (ModelState.IsValid)
            {
                bool result = await _prizeService.CreatePrizeAsync(prize);

                if (result) return Ok("The new prize was created successfully.");
            }

            return BadRequest("Something went wrong try again.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("prize/update")]
        public async Task<ActionResult> UpdatePrize([FromBody] PrizeDTO prize)
        {
            if (ModelState.IsValid)
            {
                bool result = await _prizeService.UpdatePrizeAsync(prize);

                if (result) return Ok("The update was successful.");
            }

            return BadRequest("Something went wrong try again.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("prize/delete")]
        public async Task<ActionResult> DeletePrize(Guid prizeId)
        {
            bool result = await _prizeService.DeletePrizeAsync(prizeId);

            if (result) return Ok("The prize was deleted successfully.");

            return BadRequest("Something went wrong try again.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            if (userId != null)
            {
                bool result = await _userService.DeleteUserAsync(userId);

                if (result) return Ok("The user has been successfully deleted.");
            }

            return BadRequest("Something went wrong, try again");
        }
    }
}