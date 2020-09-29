using System.Collections.Generic;
using Blaise.Api.Tests.Behaviour.Models;

namespace Blaise.Api.Tests.Behaviour.Builders
{
    public class UserModelBuilder
    {
        public UserModel BuildBasicUserModel()
        {
            return new UserModel
            {
                UserName = "TestUser1",
                Password = "TestPass",
                Role = "DST_TECH",
                ServerParks = new List<string>
                {
                    "Tel",
                    "Val"
                },
                DefaultServerPark = "Tel"
            };
        }
    }
}
