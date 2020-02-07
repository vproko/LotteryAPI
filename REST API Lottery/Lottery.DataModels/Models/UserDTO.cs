using System;
using System.Collections.Generic;

namespace Lottery.DataModels.Models
{
    public class UserDTO
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime Joined { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public IEnumerable<TicketDTO> Tickets { get; set; }
    }
}
