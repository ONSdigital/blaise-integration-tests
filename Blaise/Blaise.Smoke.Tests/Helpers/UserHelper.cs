using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Smoke.Tests.Models;
using StatNeth.Blaise.API.ServerManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaise.Smoke.Tests.Helpers
{
    public class UserHelper
    {
        private readonly IBlaiseUserApi _blaiseUserApi;
        public UserHelper()
        {
            _blaiseUserApi = new BlaiseUserApi();
        }

        public void CreateUser(UserModel userModel)
        {
            _blaiseUserApi.AddUser(userModel.UserName, userModel.Password,
                userModel.Role, userModel.ServerParks, userModel.DefaultServerPark);
        }

        public IUser GetUser(string userName)
        {
            return _blaiseUserApi.GetUser(userName);
        }

        public void RemoveUser(string userName)
        {
            _blaiseUserApi.RemoveUser(userName);
        }
    }
}
