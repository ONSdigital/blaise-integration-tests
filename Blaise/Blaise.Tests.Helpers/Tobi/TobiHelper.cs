using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Tobi.Pages;

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

        public string CheckSurveyIsDisplaying()
        {
            var homePage = new HomePage();
            return homePage.GetSurveyAcronym();
        }

        public string ClickLoadQuestionnaire()
        {
            var homePage = new HomePage();
            homePage.ClickQuestionnaireButton();
            return BrowserHelper.CurrentUrl;
        }

        public string GetFirstQuestionnaireInTable()
        {
            var surveyPage = new SurveyPage();
            return surveyPage.CheckQuestionnaireIsWithinSurveyTable();
        }
    }
}
