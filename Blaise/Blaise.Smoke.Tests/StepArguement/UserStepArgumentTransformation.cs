using Blaise.Smoke.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Blaise.Smoke.Tests.StepArguement
{
    [Binding]
    public class UserStepArgumentTransformation
    {
        [StepArgumentTransformation]
        public UserModel TransformFieldDataTableIntoAUserModel(Table table)
        {
            var userModel = table.CreateSet<UserModel>();

            return userModel.FirstOrDefault();
        }
    }
}
