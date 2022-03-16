using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Dqs.Pages
{
    public class InstrumentInfoPage : BasePage
    {
        public string ToStartDatePath = "//*[@id=\"main-content\"]/div[2]/div/table/tbody/tr/td[2]";
        public string AddToStartDatePath = "//a[contains(@href,'/questionnaire/start-date')]";
        public string DeleteButtonId = "delete-questionnaire";


        public InstrumentInfoPage() : base(DqsConfigurationHelper.DqsUrl)
        {
        }

        public string GetToStartDate()
        {
            return GetElementTextByPath(ToStartDatePath);
        }

        public void AddToStartDate()
        {
            ClickButtonByXPath(AddToStartDatePath);
        }

        public void WaitForPageToLoad(string instrumentName)
        {
            WaitForPageToChange($"{DqsConfigurationHelper.DqsUrl}/questionnaire/{instrumentName}");
        }

        public void CanDeleteQuestionnaire()
        {
            ButtonIsAvailableById(DeleteButtonId);
        }

        public void ClickDeleteButton()
        {
            ClickButtonById(DeleteButtonId);
        }
    }
}
