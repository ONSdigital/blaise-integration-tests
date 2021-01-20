using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Dqs.Pages;
using System;
using System.Collections.Generic;

namespace Blaise.Tests.Helpers.Dqs
{
    public class DqsHelper
    {
        private static DqsHelper _currentInstance;

        public static DqsHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new DqsHelper());
        }
        
        public void LoadDqsHomePage()
        {
            var homePage = new HomePage();
            homePage.LoadPage();
        }
        public List<string> GetQuestionnaireTableContents()
        {
            var homePage = new HomePage();
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
            var surveyExistsPage = new QuestionnaireExistsPage();
            surveyExistsPage.SelectCancel();
        }

        public void OverwriteQuestionnaire()
        {
            var surveyExistsPage = new QuestionnaireExistsPage();
            surveyExistsPage.SelectOverwrite();
            surveyExistsPage.SelectSaveButton();
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
            confirmDeletionPage.ClickConfirmDeleteQuestionnaireButton();
            confirmDeletionPage.ClickContinueButton();
        }

        public string GetTextForDeletion()
        {
            var homepage = new HomePage();
            return homepage.GetDeleteColumnText(BlaiseConfigurationHelper.InstrumentName);
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
            var confirmOverwritePage = new ConfirmOverwritePage();
            confirmOverwritePage.ClickConfirmOverwriteButton();
        }

        public void ConfirmSelection()
        {
            var confirmOverwritePage = new ConfirmOverwritePage();
            confirmOverwritePage.ClickContinueButton();
        }

        public void DeleteQuestionnaire(string instrumentName)
        {
            var homePage = new HomePage();
            homePage.LoadPage();
            homePage.ClickDeleteButton(instrumentName);
        }
    }
}
