﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaise.Tests.Models.User
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public List<string> ServerParks { get; set; }
        public string DefaultServerPark { get; set; }
    }
}