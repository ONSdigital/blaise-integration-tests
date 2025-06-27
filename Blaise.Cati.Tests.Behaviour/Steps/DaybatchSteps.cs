using System.Collections.Generic;
using Blaise.Tests.Helpers.Cati;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Models.Case;
using NUnit.Framework;
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
            var currentUrl = CatiManagementHelper.GetInstance().CurrentUrl();

            Assert.That(
                currentUrl,
                Is.Not.EqualTo(CatiConfigurationHelper.LoginUrl).IgnoreCase,
                $"Expected to leave the login page, but current URL is still {currentUrl}");
        }

        [Given(@"I have created a daybatch for today")]
        [When(@"I create a daybatch for today")]
        public void WhenICreateADaybatchForToday()
        {
            CatiManagementHelper.GetInstance().ClearDayBatchEntries();
            CatiManagementHelper.GetInstance().CreateDayBatch();
        }

        [Then(@"the sample cases are present on the daybatch page")]
        public void ThenTheSampleCasesArePresentOnTheDaybatchPage(IEnumerable<CaseModel> cases)
        {
            var entriesText = CatiManagementHelper.GetInstance().GetDaybatchEntriesText();

            Assert.That(
                entriesText,
                Is.Not.Null.And.Not.Empty,
                "The daybatch entries text should not be null or empty");
        }
    }
}
