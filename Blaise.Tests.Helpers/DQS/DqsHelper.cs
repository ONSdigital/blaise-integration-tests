using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Dqs.Pages;
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
            return homePage.GetTableContent();
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
    }
}
