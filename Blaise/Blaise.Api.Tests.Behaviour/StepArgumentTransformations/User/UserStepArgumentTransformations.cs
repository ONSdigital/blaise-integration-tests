using System.Linq;
using Blaise.Api.Tests.Behaviour.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Blaise.Api.Tests.Behaviour.StepArgumentTransformations.User
{
    [Binding]
    public class UserStepArgumentTransformations
    {
        [StepArgumentTransformation]
        public UserModel TransformFieldDataTableIntoAUserModel(Table table)
        {
            var userModels = table.CreateSet<UserModel>();

            return userModels.FirstOrDefault();
        }

    }
}
