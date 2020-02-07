﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.DataModels.Models
{
    public class RegisterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
