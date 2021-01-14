using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;

namespace Blaise.Tests.Helpers.DQS.Pages
{
    public class UploadPage : BasePage
    {
        private readonly string fileSelectorId = "survey-selector";
        private readonly string continueButtonId = "continue-deploy-button";
        private readonly string successCSS = "panel panel--error panel--no-title  u-mt-m";

        public UploadPage() : base(DqsConfigurationHelper.UploadUrl)
        {
        }

        public void SelectFileToUpload(string instrumentPath)
        {
            PopulateInputById(fileSelectorId, instrumentPath);
        }

        public void SelectContinueButton()
        {
            ClickButtonById(continueButtonId);
        }

        public void WaitForUploadCompletion()
        {
            WaitForPageToChange(DqsConfigurationHelper.UploadSummaryUrl);
        }
    }
}
