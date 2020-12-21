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
            var surveyPage = new QuestionnairePage();
            return surveyPage.GetTableContent().FirstOrDefault();
        }

        public void LoadSurveyPage()
        {
            var surveyPage = new QuestionnairePage();
            surveyPage.LoadPage();
        }

        public string ClickInterviewButton()
        {
            var surveyPage = new QuestionnairePage();
            surveyPage.ClickInterviewButton();
            return BrowserHelper.CurrentUrl;
        }

        public List<string> GetQuestionnaireTableContents()
        {
            var surveyPage = new QuestionnairePage();
            var n = surveyPage.GetTableContent();
            return n;
        }
    }
}
