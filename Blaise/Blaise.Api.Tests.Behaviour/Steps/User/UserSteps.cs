using Blaise.Api.Tests.Behaviour.Builders;
using Blaise.Api.Tests.Behaviour.Enums;
using Blaise.Api.Tests.Behaviour.Helpers;
using Blaise.Api.Tests.Behaviour.Models;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Api.Tests.Behaviour.Steps.User
{
    [Binding, Scope(Tag = "user")]
    public sealed class UserSteps
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext _scenarioContext;

        private readonly UserDataHelper _userDataHelper;
        private readonly UserModelBuilder _userModelBuilder;

        public UserSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;

            _userDataHelper = new UserDataHelper();
            _userModelBuilder = new UserModelBuilder();
        }

        [Given(@"I have a new user I want to add")]
        public void GivenIHaveANewUserIWantToAdd()
        {
            var userModel = _userModelBuilder.BuildBasicUserModel();
       
            _scenarioContext.Set(userModel, ScenarioContextType.User.ToString());
        }

        [When(@"I add the user")]
        public void WhenIAddTheUser()
        {
            var userModel = _scenarioContext.Get<UserModel>(ScenarioContextType.User.ToString());

            _userDataHelper.CreateUser(userModel);
        }

        [Given(@"a user exists in blaise")]
        public void GivenAUserExistsInBlaise(UserModel userModel)
        {
            _userDataHelper.CreateUser(userModel);

            _scenarioContext.Set(userModel, ScenarioContextType.User.ToString());
        }

        [Then(@"the user is successfully added")]
        public void ThenTheUserIsSuccessfullyAdded()
        {
            var userModel = _scenarioContext.Get<UserModel>(ScenarioContextType.User.ToString());

            var exists = _userDataHelper.CheckUserExists(userModel.UserName);

            Assert.IsTrue(exists);
        }

        [When(@"I retrieve the user")]
        public void WhenIRetrieveTheUser()
        {
            var userModel = _scenarioContext.Get<UserModel>(ScenarioContextType.User.ToString());
            userModel = _userDataHelper.GetUser(userModel.UserName);

            _scenarioContext.Set(userModel, ScenarioContextType.User.ToString());
        }

        [Then(@"the correct user is returned")]
        public void ThenTheCorrectUserIsReturned(UserModel expectedUserModel)
        {
            var actualUserModel = _scenarioContext.Get<UserModel>(ScenarioContextType.User.ToString());

            Assert.AreEqual(expectedUserModel.UserName, actualUserModel.UserName);
            Assert.AreEqual(expectedUserModel.Password, actualUserModel.Password);
            Assert.AreEqual(expectedUserModel.Role, actualUserModel.Role);
            Assert.AreEqual(expectedUserModel.DefaultServerPark, actualUserModel.DefaultServerPark);
            Assert.AreEqual(expectedUserModel.ServerParks.Count, actualUserModel.ServerParks.Count);

            foreach (var serverPark in expectedUserModel.ServerParks)
            {
                Assert.IsTrue(actualUserModel.ServerParks.Exists(s => s.Contains(serverPark)));
            }
        }

        [AfterScenario]
        public void CleanUpDataAfter()
        {
            var userModel = _scenarioContext.Get<UserModel>(ScenarioContextType.User.ToString());

            _userDataHelper.DeleteUser(userModel.UserName);
        }
    }
}
