using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Tobi.Pages;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Tobi
{
    public class TobiHelper
    {
        private static TobiHelper _currentInstance;

        public static TobiHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new TobiHelper());
        }

        public void LoadTobiHomePage()
        {
            var homePage = new HomePage();
            homePage.LoadPage();
        }

        public List<string> GetSurveyTableContents()
        {
            var homePage = new HomePage();
            return homePage.GetSurveyAcronyms();
        }

        public string ClickLoadQuestionnaire()
        {
            var homePage = new HomePage();
            homePage.ClickQuestionnaireButton();
            return BrowserHelper.CurrentUrl;
        }

        public string GetFirstQuestionnaireInTable()
        {
            var questionnairePage = new QuestionnairePage();
            return questionnairePage.GetTableContent().FirstOrDefault();
        }

        public void LoadQuestionnairePagePage()
        {
            var questionnairePage = new QuestionnairePage();
            questionnairePage.LoadPage();
        }

        public string ClickInterviewButton()
        {
            var questionnairePage = new QuestionnairePage();
            questionnairePage.ClickInterviewButton();
            return BrowserHelper.CurrentUrl;
        }

        public List<string> GetQuestionnaireTableContents()
        {
            var questionnairePage = new QuestionnairePage();
            return questionnairePage.GetTableContent();
        }

        public void ClickReturnToSurveyListButton()
        {
            var questionnairePage = new QuestionnairePage();
            questionnairePage.ClickReturnToSurveyList();
        }
    }
}
