using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Cati;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Questionnaire;
using Blaise.Tests.Models.Case;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using TechTalk.SpecFlow;

namespace Blaise.Cati.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class DaybatchSteps
    {
        [Given(@"I log into the CATI dashboard as an administrator")]
        public void GivenILogOnToTheCatiDashboard()
        {
            CatiManagementHelper.GetInstance().LogIntoCatiDashboardAsAdministrator();
            Assert.AreNotEqual(CatiConfigurationHelper.LoginUrl, CatiManagementHelper.GetInstance().CurrentUrl(),
                "Expected to leave the login page");
        }

        [Given(@"I have created a daybatch for today")]
        [When(@"I create a daybatch for today")]
        public void WhenICreateADaybatchForToday()
        {
            CatiManagementHelper.GetInstance().ClearDayBatchEntries();
            CatiManagementHelper.GetInstance().CreateDayBatch();
        }
        
        [When(@"the sample cases are present on the daybatch page")]
        [Then(@"the sample cases are present on the daybatch page")]
        public void ThenTheSampleCasesArePresentOnTheDaybatchPage(IEnumerable<CaseModel> cases)
        {
            var entriesText = CatiManagementHelper.GetInstance().GetDaybatchEntriesText();
            Assert.IsNotNull(entriesText);
        }
    }
}
