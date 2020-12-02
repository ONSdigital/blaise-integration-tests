using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Tests.Models.User;

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
        
        public void RemoveUser(string userName)
        {
            _blaiseUserApi.RemoveUser(userName);
        }
    }
}
