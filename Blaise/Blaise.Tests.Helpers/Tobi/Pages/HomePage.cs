using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;

namespace Blaise.Tests.Helpers.Tobi.Pages
{
    public class HomePage : BasePage
    {
        public string SurveyLetterPath = "//*[@id='basic-table']/tbody/tr/td[1]";
        public string LaunchQuestionnaireLinkPath = "//*[@id='basic-table']/tbody/tr/td[2]/a";
        public HomePage() : base(TobiConfigurationHelper.TobiUrl)
        {
        }

        public string GetSurveyAcronym()
        {
            return GetElementTextByPath(SurveyLetterPath);
        }

        public void ClickQuestionnaireButton()
        {
            ClickButtonByXPath(LaunchQuestionnaireLinkPath);
        }
    }
}
