using System;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Dqs;
using Blaise.Tests.Helpers.Questionnaire;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Dqs.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class ToStartDateSteps
    {
        [BeforeScenario("to-start-date")]
        public static void BeforeScenario()
        {
            QuestionnaireHelper.GetInstance().InstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName, BlaiseConfigurationHelper.QuestionnairePath, BlaiseConfigurationHelper.QuestionnaireInstallOptions);
        }

        [Given(@"a questionnaire has been deployed")]
        public void GivenAQuestionnaireHasBeenDeployed()
        {
        }

        [Given(@"the questionnaire has no TO start date")]
        [Then(@"the TO start date should not be set")]
        public void GivenTheQuestionnaireHasNoToStartDate()
        {
            DqsHelper.GetInstance().ClickQuestionnaireInfoButton(BlaiseConfigurationHelper.QuestionnaireName);
            var toStartDateText = DqsHelper.GetInstance().GetToStartDate();

            Assert.That(
                toStartDateText,
                Is.EqualTo("No start date specified, using survey days"),
                "The TO start date should indicate that no start date is specified");
        }

        [Given(@"the questionnaire has a start date of '(.*)'")]
        [When(@"I change the TO start date to '(.*)'")]
        public void GivenTheQuestionnaireHasAStartDateOf(string date)
        {
            DqsHelper.GetInstance().ClickQuestionnaireInfoButton(BlaiseConfigurationHelper.QuestionnaireName);
            var toStartDate = DateTime.Now.ToString("dd/MM/yyyy");
            if (date == "tomorrow")
            {
                toStartDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            }

            DqsHelper.GetInstance().ClickAddStartDate();
            DqsHelper.GetInstance().SelectYesLiveDate();
            DqsHelper.GetInstance().SetLiveDate(toStartDate);
            DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
        }

        [When(@"I add a TO start date of '(.*)'")]
        public void WhenIAddAToStartDateOf(string date)
        {
            var toStartDate = DateTime.Now.ToString("dd/MM/yyyy");
            if (date == "tomorrow")
            {
                toStartDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            }

            DqsHelper.GetInstance().ClickAddStartDate();
            DqsHelper.GetInstance().SelectYesLiveDate();
            DqsHelper.GetInstance().SetLiveDate(toStartDate);
            DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
        }

        [When(@"I remove the TO start date")]
        public void WhenIRemoveTheToStartDate()
        {
            DqsHelper.GetInstance().ClickQuestionnaireInfoButton(BlaiseConfigurationHelper.QuestionnaireName);
            DqsHelper.GetInstance().ClickAddStartDate();
            DqsHelper.GetInstance().SelectNoToStartDate();
            DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
        }

        [Then(@"the TO start date for '(.*)' is stored against the questionnaire")]
        public void ThenTheToStartDateForIsStoredAgainstTheQuestionnaire(string date)
        {
            var toStartDate = date == "tomorrow"
                ? DateTime.Now.AddDays(1).ToString("dd/MM/yyyy")
                : DateTime.Now.ToString("dd/MM/yyyy");

            DqsHelper.GetInstance().ClickQuestionnaireInfoButton(BlaiseConfigurationHelper.QuestionnaireName);
            var toStartDateText = DqsHelper.GetInstance().GetToStartDate();

            Assert.That(
                toStartDateText,
                Does.Contain(toStartDate),
                $"TO start date text should contain '{toStartDate}', but got: '{toStartDateText}'");
        }
    }
}
