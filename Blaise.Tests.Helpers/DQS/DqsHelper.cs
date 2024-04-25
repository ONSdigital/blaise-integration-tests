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

        public void LogInToDqs(string username, string password)
        {
            var loginPage = new LoginPage();
            loginPage.LoadPage();
            loginPage.LogIntoDqs(username, password);
        }

        public void LogOutOfToDqs()
        {
            var loginPage = new LoginPage();
            loginPage.LogOutOfDqs();
        }

        public void LoadDqsHomePage()
        {
            var homePage = new HomePage();
            homePage.LoadPage();
        }

        public List<string> GetQuestionnaireTableContents()
        {
            var homePage = new HomePage();
            homePage.FilterInstruments(BlaiseConfigurationHelper.InstrumentName);
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
            uploadPage.SelectFileToUpload(BlaiseConfigurationHelper.InstrumentPackage);
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
            var instrumentInfoPage = new QuestionnaireInfoPage();
            instrumentInfoPage.CanDeleteQuestionnaire();
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

        public void SelectNoLiveDate()
        {
            var uploadPage = new UploadPage();
            uploadPage.SelectNoLiveDateButton();
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

        public void DeleteQuestionnaire(string instrumentName)
        {
            ClickInstrumentInfoButton(instrumentName);
            var instrumentInformationPage = new QuestionnaireInfoPage();
            Thread.Sleep(5000);
            instrumentInformationPage.ClickDeleteButton();
        }

        public string GetDeletionSummary()
        {
            var homePage = new HomePage();
            homePage.FilterInstruments(BlaiseConfigurationHelper.InstrumentName);
            return homePage.GetUploadSummaryText();
        }

        public string GetLivedateSummaryText()
        {
            var uploadPage = new UploadPage();
            return uploadPage.GetLiveDateSummaryText();
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
            var instrumentInfoPage = new QuestionnaireInfoPage();
            return instrumentInfoPage.GetToStartDate();
        }

        public void ClickInstrumentInfoButton(string instrumentName)
        {
            var homePage = new HomePage();
            homePage.LoadPage();
            homePage.FilterInstruments(BlaiseConfigurationHelper.InstrumentName);
            homePage.ClickInstrumentInfoButton(instrumentName);
        }

        public void ClickAddStartDate()
        {
            var instrumentInfoPage = new QuestionnaireInfoPage();
            instrumentInfoPage.AddToStartDate();
        }

        public void WaitForQuestionnaireDetailsPage()
        {
            var instrumentInfoPage = new QuestionnaireInfoPage();
            instrumentInfoPage.WaitForPageToLoad(BlaiseConfigurationHelper.InstrumentName);
        }

        public void WaitForDeleteQuestionnaireConfirmationPage()
        {
            var confirmDeletionPage = new DeleteConfirmationPage();
            confirmDeletionPage.WaitForPageToLoad();
        }
    }
}
