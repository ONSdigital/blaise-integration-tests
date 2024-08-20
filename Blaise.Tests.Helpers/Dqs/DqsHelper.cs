using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Dqs.Pages;
using System.Collections.Generic;
using System.Threading;

namespace Blaise.Tests.Helpers.Dqs
{
    public class DqsHelper
    {
        private static DqsHelper _currentInstance;

        public static DqsHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new DqsHelper());
        }

        public void LogIntoDqs(string username, string password)
        {
            var loginPage = new LoginPage();
            loginPage.LoadPage();
            loginPage.LogIntoDqs(username, password);
        }

        public void LogoutOfDqs()
        {
            var loginPage = new LoginPage();
            loginPage.LogoutOfDqs();
        }

        public bool IsLogoutButtonVisible()
        {
            var loginPage = new LoginPage();
            return loginPage.IsLogoutButtonVisible();
        }

        public void LoadDqsHomePage()
        {
            var homePage = new HomePage();
            homePage.LoadPage();
        }

        public List<string> GetQuestionnaireTableContents()
        {
            var homePage = new HomePage();
            homePage.FilterQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName);
            return homePage.GetFirstColumnFromTableContent();
        }

        public void ClickDeployQuestionnaire()
        {
            var homePage = new HomePage();
            homePage.ClickDeployAQuestionnaire();
        }

        public void LoadUploadPage()
        {
            var uploadPage = new UploadPage();
            uploadPage.LoadPage();
        }

        public void CancelDeploymentOfQuestionnaire()
        {
            var uploadPage = new UploadPage();
            uploadPage.SelectCancelButton();
        }

        public void OverwriteQuestionnaire()
        {
            var surveyExistsPage = new UploadPage();
            surveyExistsPage.SelectContinueOverwriteButton();
            surveyExistsPage.SelectContinueButton();
        }

        public void SelectQuestionnairePackage()
        {
            var uploadPage = new UploadPage();
            uploadPage.SelectFileToUpload(BlaiseConfigurationHelper.QuestionnairePackage);
        }

        public void ConfirmQuestionnaireUpload()
        {
            var uploadPage = new UploadPage();
            uploadPage.SelectContinueButton();
        }

        public void ConfirmDeletionOfQuestionnaire()
        {
            var confirmDeletionPage = new DeleteConfirmationPage();
            confirmDeletionPage.ClickContinueButton();
        }

        public void WaitForDeletionToComplete()
        {
            var confirmDeletionPage = new DeleteConfirmationPage();
            confirmDeletionPage.WaitForDeletionToComplete();
        }

        public void CanDeleteQuestionnaire()
        {
            var questionnaireInfoPage = new QuestionnaireInfoPage();
            questionnaireInfoPage.CanDeleteQuestionnaire();
        }

        public void WaitForUploadToComplete()
        {
            var uploadPage = new UploadPage();
            uploadPage.WaitForUploadCompletion();
        }

        public string GetUploadMessage()
        {
            var uploadSummaryPage = new UploadSummaryPage();
            return uploadSummaryPage.GetUploadSummaryText();
        }

        public void SelectNoToStartDate()
        {
            var uploadPage = new UploadPage();
            uploadPage.SelectNoToStartDateButton();
        }

        public void WaitForQuestionnaireAlreadyExistsPage()
        {
            var uploadPage = new UploadPage();
            uploadPage.WaitForQuestionnaireAlreadyExistsPage();
        }

        public string GetOverwriteMessage()
        {
            var cannotOverwritePage = new CannotOverwritePage();
            return cannotOverwritePage.GetUploadSummaryText();
        }

        public void ConfirmOverwriteOfQuestionnaire()
        {
            var confirmOverwritePage = new UploadPage();
            confirmOverwritePage.SelectYesLiveDateButton();

        }

        public void DeleteQuestionnaire(string questionnaireName)
        {
            ClickQuestionnaireInfoButton(questionnaireName);
            var questionnaireInformationPage = new QuestionnaireInfoPage();
            Thread.Sleep(5000);
            questionnaireInformationPage.ClickDeleteButton();
        }

        public string GetDeletionSummary()
        {
            var homePage = new HomePage();
            homePage.FilterQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName);
            return homePage.GetUploadSummaryText();
        }

        public string GetToStartDateSummaryText()
        {
            var uploadPage = new UploadPage();
            return uploadPage.GetToStartDateSummaryText();
        }

        public void SelectYesLiveDate()
        {
            var uploadPage = new UploadPage();
            uploadPage.SelectYesLiveDateButton();
        }

        public void SetLiveDate(string date)
        {
            var uploadPage = new UploadPage();
            uploadPage.SetLiveDate(date);
        }
        public string GetToStartDate()
        {
            var questionnaireInfoPage = new QuestionnaireInfoPage();
            return questionnaireInfoPage.GetToStartDate();
        }

        public void ClickQuestionnaireInfoButton(string questionnaireName)
        {
            var homePage = new HomePage();
            homePage.LoadPage();
            homePage.FilterQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName);
            homePage.ClickQuestionnaireInfoButton(questionnaireName);
        }

        public void ClickAddStartDate()
        {
            var questionnaireInfoPage = new QuestionnaireInfoPage();
            questionnaireInfoPage.AddToStartDate();
        }

        public void WaitForQuestionnaireDetailsPage()
        {
            var questionnaireInfoPage = new QuestionnaireInfoPage();
            questionnaireInfoPage.WaitForPageToLoad(BlaiseConfigurationHelper.QuestionnaireName);
        }

        public void WaitForDeleteQuestionnaireConfirmationPage()
        {
            var confirmDeletionPage = new DeleteConfirmationPage();
            confirmDeletionPage.WaitForPageToLoad();
        }
    }
}
