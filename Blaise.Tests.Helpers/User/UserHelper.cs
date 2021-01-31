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
            if (GetUser(userModel.UserName) == null)
            {
                _blaiseUserApi.AddUser(userModel.UserName, userModel.Password,
                    userModel.Role, userModel.ServerParks, userModel.DefaultServerPark);
            }
            else
            {
                RemoveUser(userModel.UserName);

                _blaiseUserApi.AddUser(userModel.UserName, userModel.Password,
                   userModel.Role, userModel.ServerParks, userModel.DefaultServerPark);
            }    
        }

        public void RemoveUser(string userName)
        {
            _blaiseUserApi.RemoveUser(userName);
        }

        public IUser GetUser(string userName)
        {
            try
            {
                return _blaiseUserApi.GetUser(userName);
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}
