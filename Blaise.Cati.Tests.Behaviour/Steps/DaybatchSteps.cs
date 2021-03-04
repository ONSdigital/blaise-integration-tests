using System;
using System.Collections.Generic;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Cati;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Instrument;
using Blaise.Tests.Models.Case;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Cati.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class DaybatchSteps
    {
        [BeforeFeature("cati")]
        public static void InitializeFeature()
        {
            InstrumentHelper.GetInstance().InstallInstrument();
        }

        [Given(@"I log on to Cati as an administrator")]
        public void GivenILogOnToTheCatiDashboard()
        {
            try
            {
                CatiManagementHelper.GetInstance().LogIntoCatiManagementPortal();
                Assert.AreNotEqual(CatiConfigurationHelper.LoginUrl, CatiManagementHelper.GetInstance().CurrentUrl(),
                    "Expected to leave the login page");
            }
            catch (Exception e)
            {
                FailWithScreenShot(e,"LogOnCati", "Log onto Cati as Admin");
            }
        }

        [Given(@"I have created a daybatch for today")]
        [When(@"I create a daybatch for today")]
        public void WhenICreateADaybatchForToday()
        {
            try
            {
                CatiManagementHelper.GetInstance().CreateDayBatch();
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "CreateDaybatch", "Create a daybatch for today");
            }
        }
        [When(@"the sample cases are present on the daybatch entry screen")]
        [Then(@"the sample cases are present on the daybatch entry screen")]
        public void ThenTheSampleCasesArePresentOnTheDaybatchEntryScreen(IEnumerable<CaseModel> cases)
        {
            try
            {
                var entriesText = CatiManagementHelper.GetInstance().GetDaybatchEntriesText();
                Assert.IsNotNull(entriesText);
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "SampleCases", "Daybatch entry screen");
            }
        }
                
        [AfterScenario("cati")]
        public void CleanUpFeature()
        {
            CatiManagementHelper.GetInstance().ClearDayBatchEntries();
            BrowserHelper.CloseBrowser();
            CaseHelper.GetInstance().DeleteCases();
            InstrumentHelper.GetInstance().UninstallSurvey();
        }

        private static void FailWithScreenShot(Exception e, string screenShotName, string screenShotDescription)
        {
            var screenShotFile = BrowserHelper.TakeScreenShot(TestContext.CurrentContext.WorkDirectory,
                screenShotName);

            TestContext.AddTestAttachment(screenShotFile, screenShotDescription);
            Assert.Fail($"The test failed to complete - {e.Message}");
        }
    }
}
