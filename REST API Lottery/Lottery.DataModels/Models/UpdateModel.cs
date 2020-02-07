using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.DataModels.Models
{
    public class UpdateModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmedPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
