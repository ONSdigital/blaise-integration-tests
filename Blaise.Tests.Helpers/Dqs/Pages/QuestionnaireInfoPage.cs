namespace Blaise.Tests.Helpers.Dqs.Pages
{
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class QuestionnaireInfoPage : BasePage
    {
        private const string ToStartDatePath = "//div[contains(@class,'ons-summary__item')][.//div[normalize-space()='Telephone Operations start date']]//span[contains(@class,'ons-summary__text')]";
        private const string AddToStartDatePath = "//a[contains(@href,'/to-start-date')]";
        private const string DeleteButtonId = "delete-questionnaire";

        public QuestionnaireInfoPage()
            : base(DqsConfigurationHelper.DqsUrl)
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

        public void WaitForPageToLoad(string questionnaireName)
        {
            WaitForPageToChange($"{DqsConfigurationHelper.DqsUrl}/questionnaire/{questionnaireName}");
        }

        public void CanDeleteQuestionnaire()
        {
            ButtonIsAvailableById(DeleteButtonId);
        }

        public void ClickDeleteButton()
        {
            ClickButtonById(DeleteButtonId);
        }

        protected override By PageIdentityBy => By.Id(DeleteButtonId);
    }
}
