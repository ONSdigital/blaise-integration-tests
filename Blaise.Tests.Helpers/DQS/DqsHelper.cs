using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.DQS.Pages;

namespace Blaise.Tests.Helpers.DQS
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
