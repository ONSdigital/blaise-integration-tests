using Blaise.Api.Tests.Behaviour.Models;
using Blaise.Nuget.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Api.Tests.Behaviour.Helpers
{
    public class UserDataHelper
    {
        private readonly IBlaiseApi _blaiseApi;

        private readonly ConnectionModel _connectionModel;
        

        public UserDataHelper()
        {
            _blaiseApi = new BlaiseApi();

            _connectionModel = _blaiseApi.GetDefaultConnectionModel();
        }

        public void CreateUser(UserModel userModel)
        {
            _blaiseApi.AddUser(_connectionModel, userModel.UserName, userModel.Password, userModel.Role,
                userModel.ServerParks, userModel.DefaultServerPark);
        }

        public bool CheckUserExists(string userName)
        {
            return _blaiseApi.UserExists(_connectionModel, userName);
        }

        public void DeleteUser(string userName)
        {
            _blaiseApi.RemoveUser(_connectionModel, userName);
        }

        public UserModel GetUser(string userName)
        {
          return new UserModel();
        }
    }
}
