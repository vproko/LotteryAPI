using System;
using System.Collections.Generic;

namespace Lottery.DomainClasses.Models
{
    public class User
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


        public virtual IEnumerable<Ticket> Tickets { get; set; }
    }
}
