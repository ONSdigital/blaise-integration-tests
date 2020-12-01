using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Tests.Models.User;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Tests.Helpers.User
{
    public class UserHelper
    {
        private readonly IBlaiseUserApi _blaiseUserApi;
        private static UserHelper _currentInstance;

        public UserHelper()
        {
            _blaiseUserApi = new BlaiseUserApi();
        }

        public static UserHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new UserHelper());
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
