namespace Blaise.Tests.Helpers.Dqs.Pages
{
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;

    public class QuestionnaireInfoPage : BasePage
    {
        private const string _toStartDatePath = "//*[@id=\"main-content\"]/div[2]/div/table/tbody/tr/td[2]";
        private const string _addToStartDatePath = "//a[contains(@href,'/questionnaire/start-date')]";
        private const string _deleteButtonId = "delete-questionnaire";

        public QuestionnaireInfoPage()
            : base(DqsConfigurationHelper.DqsUrl)
        {
        }

        public string GetToStartDate()
        {
            return GetElementTextByPath(_toStartDatePath);
        }

        public void AddToStartDate()
        {
            ClickButtonByXPath(_addToStartDatePath);
        }

        public void WaitForPageToLoad(string questionnaireName)
        {
            WaitForPageToChange($"{DqsConfigurationHelper.DqsUrl}/questionnaire/{questionnaireName}");
        }

        public void CanDeleteQuestionnaire()
        {
            ButtonIsAvailableById(_deleteButtonId);
        }

        public void ClickDeleteButton()
        {
            ClickButtonById(_deleteButtonId);
        }
    }
}
