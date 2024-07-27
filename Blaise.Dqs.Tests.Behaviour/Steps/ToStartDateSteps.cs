using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Dqs;
using Blaise.Tests.Helpers.Questionnaire;
using NUnit.Framework;
using System;
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
            QuestionnaireHelper.GetInstance().InstallInstrument();
        }

        [Given(@"A questionnaire is installed in Blaise")]
        public void GivenAnInstrumentIsInstalledInBlaise()
        {
        }

        [Given(@"The instrument has no TO start date")]
        [Then(@"The TO start date should not be set")]
        public void GivenTheInstrumentHasNoTOStartDate()
        {
            DqsHelper.GetInstance().ClickInstrumentInfoButton(BlaiseConfigurationHelper.InstrumentName);
            var toStartDateText = DqsHelper.GetInstance().GetToStartDate();

            Assert.AreEqual("No start date specified, using survey days", toStartDateText);
        }

        [Given(@"The instrument has a start date of '(.*)'")]
        [When(@"I change the TO start date to '(.*)'")]
        public void GivenTheInstrumentHasAStartDateOf(string date)
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

        [When(@"I add a TO start date of '(.*)'")]
        public void WhenIAddATOStartDateOf(string date)
        {
            var toStartDate = DateTime.Now.ToString("dd/MM/yyyy");
            if (date == "tomorrow")
                toStartDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            DqsHelper.GetInstance().ClickAddStartDate();
            DqsHelper.GetInstance().SelectYesLiveDate();
            DqsHelper.GetInstance().SetLiveDate(toStartDate);
            DqsHelper.GetInstance().ConfirmQuestionnaireUpload(); 
        }

        [When(@"I change the TO start date to no TO start date")]
        public void WhenIChangeTheTOStartDateToNoTOStartDate()
        {
            DqsHelper.GetInstance().ClickInstrumentInfoButton(BlaiseConfigurationHelper.InstrumentName);
            DqsHelper.GetInstance().ClickAddStartDate();
            DqsHelper.GetInstance().SelectNoLiveDate();
            DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
        }

        [Then(@"The TO start date for '(.*)' is stored against the questionnaire")]
        public void ThenTheTOStartDateForIsStoredAgainstTheInstrument(string date)
        {
            var toStartDate = DateTime.Now.ToString("dd/MM/yyyy");
            if (date == "tomorrow")
                toStartDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            DqsHelper.GetInstance().ClickInstrumentInfoButton(BlaiseConfigurationHelper.InstrumentName);
            var toStartDateText = DqsHelper.GetInstance().GetToStartDate();

            Assert.IsTrue(toStartDateText.Contains(toStartDate));
        }

        [AfterScenario("ToStartDate")]
        public void CleanUpScenario()
        {
            DqsHelper.GetInstance().LogOutOfToDqs();
            if (QuestionnaireHelper.GetInstance().SurveyExists(BlaiseConfigurationHelper.InstrumentName))
            {
                CaseHelper.GetInstance().DeleteCases();
                QuestionnaireHelper.GetInstance().UninstallSurvey();
            }
            BrowserHelper.ClosePreviousTab();
        }
    }
}
