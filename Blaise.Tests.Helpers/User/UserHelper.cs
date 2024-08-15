using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Tests.Models.User;
using StatNeth.Blaise.API.ServerManager;
using System;
using System.Net;

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
            if (GetUser(userModel.Username) == null)
            {
                _blaiseUserApi.AddUser(userModel.Username, userModel.Password,
                    userModel.Role, userModel.ServerParks, userModel.DefaultServerPark);
            }
            else
            {
                RemoveUser(userModel.Username);

                _blaiseUserApi.AddUser(userModel.Username, userModel.Password,
                   userModel.Role, userModel.ServerParks, userModel.DefaultServerPark);
            }
        }

        public void RemoveUser(string userName)
        {
            try
            {
                _blaiseUserApi.RemoveUser(userName);
            }
            catch (WebException ex) when (ex.Message.Contains("Bad Request"))
            {
                // The remote server returned an unexpected response: (400) Bad Request.
                // These are not thrown as if the user cannot be removed it will not affect the system
            }
            catch (Exception)
            {
                // Handle other exceptions.
                // These are not thrown as if the user cannot be removed it will not affect the system
            }
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
