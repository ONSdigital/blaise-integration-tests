using System.Collections.Generic;

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
