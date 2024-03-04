﻿using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Cati;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Instrument;
using Blaise.Tests.Models.Case;
using NUnit.Framework;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Blaise.Cati.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class DaybatchSteps
    {
        private static ScenarioContext _scenarioContext;
        public DaybatchSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeFeature("cati")]
        public static void InitializeFeature()
        {
            if (_scenarioContext.TestError == null)
                return;

            CatiManagementHelper.GetInstance().CreateAdminUser();
            InstrumentHelper.GetInstance().InstallInstrument();
        }

        [Given(@"I log on to Cati as an administrator")]
        public void GivenILogOnToTheCatiDashboard()
        {
            CatiManagementHelper.GetInstance().LogIntoCatiManagementPortal();
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
        [When(@"the sample cases are present on the daybatch entry screen")]
        [Then(@"the sample cases are present on the daybatch entry screen")]
        public void ThenTheSampleCasesArePresentOnTheDaybatchEntryScreen(IEnumerable<CaseModel> cases)
        {
            var entriesText = CatiManagementHelper.GetInstance().GetDaybatchEntriesText();
            Assert.IsNotNull(entriesText);
        }

        [AfterScenario("cati")]
        public void CleanUpFeature()
        {
            CatiManagementHelper.GetInstance().ClearDayBatchEntries();
            CatiManagementHelper.GetInstance().DeleteAdminUser();
            CaseHelper.GetInstance().DeleteCases();
            InstrumentHelper.GetInstance().UninstallSurvey();
            BrowserHelper.ClearSessionData();
        }
    }
}
