using System.Collections.Generic;

namespace Blaise.Api.Tests.Behaviour.Models
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
