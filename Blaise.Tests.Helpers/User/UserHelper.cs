namespace Blaise.Tests.Helpers.User
{
    using System;
    using System.Net;
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Interfaces;
    using Blaise.Tests.Models.User;
    using StatNeth.Blaise.API.ServerManager;

    public class UserHelper
    {
        private static UserHelper _currentInstance;
        private readonly IBlaiseUserApi _blaiseUserApi;

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
                _blaiseUserApi.AddUser(userModel.Username, userModel.Password, userModel.Role, userModel.ServerParks, userModel.DefaultServerPark);
            }
            else
            {
                RemoveUser(userModel.Username);

                _blaiseUserApi.AddUser(userModel.Username, userModel.Password, userModel.Role, userModel.ServerParks, userModel.DefaultServerPark);
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
                Console.WriteLine($"Warning: Failed to remove user '{userName}' (Bad Request). They may not exist. Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Failed to remove user '{userName}'. Error: {ex.Message}");
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
