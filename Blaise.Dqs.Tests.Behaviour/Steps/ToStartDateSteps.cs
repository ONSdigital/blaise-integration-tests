using System;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Dqs;
using Blaise.Tests.Helpers.Instrument;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Dqs.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class ToStartDateSteps
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        [BeforeScenario("ToStartDate")]
        public static void InitializeScenario()
        {
            InstrumentHelper.GetInstance().InstallInstrument();
        }

        [Given(@"An instrument is installed in Blaise")]
        public void GivenAnInstrumentIsInstalledInBlaise()
        {
        }

        [Given(@"The instrument has no TO start date")]
        [Then(@"The TO start date should not be set")]
        public void GivenTheInstrumentHasNoTOStartDate()
        {
            try
            {
                DqsHelper.GetInstance().ClickInstrumentInfoButton(BlaiseConfigurationHelper.InstrumentName);
                string toStartDateText = DqsHelper.GetInstance().GetToStartDate();

                Assert.AreEqual("No start date specified, using survey days", toStartDateText);
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "GettingTOStartDate", "Getting the to start date");
            }
        }

        [Given(@"The instrument has a start date of '(.*)'")]
        [When(@"I change the TO start date to '(.*)'")]
        public void GivenTheInstrumentHasAStartDateOf(string date)
        {
            try
            {
                DqsHelper.GetInstance().ClickInstrumentInfoButton(BlaiseConfigurationHelper.InstrumentName);
                var toStartDate = DateTime.Now.ToString("dd/MM/yyyy");
                if (date == "tomorrow")
                    toStartDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                DqsHelper.GetInstance().ClickAddStartDate();
                DqsHelper.GetInstance().SelectYesLiveDate();
                DqsHelper.GetInstance().SetLiveDate(toStartDate);
                DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "AddingTOStartDate", "Adding the to start date");
            }
        }

        [When(@"I add a TO start date of '(.*)'")]
        public void WhenIAddATOStartDateOf(string date)
        {
            try
            {
                var toStartDate = DateTime.Now.ToString("dd/MM/yyyy");
                if (date == "tomorrow")
                    toStartDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                DqsHelper.GetInstance().ClickAddStartDate();
                DqsHelper.GetInstance().SelectYesLiveDate();
                DqsHelper.GetInstance().SetLiveDate(toStartDate);
                DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "AddingTOStartDate", "Adding the to start date");
            }
        }

        [When(@"I change the TO start date to no TO start date")]
        public void WhenIChangeTheTOStartDateToNoTOStartDate()
        {
            try
            {
                DqsHelper.GetInstance().ClickInstrumentInfoButton(BlaiseConfigurationHelper.InstrumentName);
                DqsHelper.GetInstance().ClickAddStartDate();
                DqsHelper.GetInstance().SelectNoLiveDate();
                DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "RemoveStartDate", "Removing the to start date");
            }
        }

        [Then(@"The TO start date for '(.*)' is stored against the instrument")]
        public void ThenTheTOStartDateForIsStoredAgainstTheInstrument(string date)
        {
            try
            {
                var toStartDate = DateTime.Now.ToString("dd/MM/yyyy");
                if (date == "tomorrow")
                    toStartDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                DqsHelper.GetInstance().ClickInstrumentInfoButton(BlaiseConfigurationHelper.InstrumentName);
                string toStartDateText = DqsHelper.GetInstance().GetToStartDate();

                Assert.IsTrue(toStartDateText.Contains(toStartDate));
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "GettingTOStartDate", "Getting the to start date");
            }
        }

        private static void FailWithScreenShot(Exception e, string screenShotName, string screenShotDescription)
        {
            var screenShotFile = BrowserHelper.TakeScreenShot(TestContext.CurrentContext.WorkDirectory,
                screenShotName);

            TestContext.AddTestAttachment(screenShotFile, screenShotDescription);
            Assert.Fail($"The test failed to complete - {e.Message}");
        }

        [AfterScenario("TOStartDate")]
        public void CleanUpScenario()
        {
            BrowserHelper.CloseBrowser();
            if (InstrumentHelper.GetInstance().SurveyExists(BlaiseConfigurationHelper.InstrumentName))
            {
                CaseHelper.GetInstance().DeleteCases();
                InstrumentHelper.GetInstance().UninstallSurvey();
            }
        }
    }
}
