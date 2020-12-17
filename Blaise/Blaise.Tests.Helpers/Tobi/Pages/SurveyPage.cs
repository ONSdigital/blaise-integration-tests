using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Tobi.Pages
{
    public class SurveyPage : BasePage
    {
        protected string QuestionnaireTableId = "basic-table";
        public string QuestionnaireNamePath = "//*[@id='basic-table']/tbody/tr/td[1]";

        public SurveyPage() : base(TobiConfigurationHelper.SurveyUrl)
        {
        }

        public string CheckQuestionnaireIsWithinSurveyTable()
        {
            return GetElementTextByPath(QuestionnaireNamePath);
        }
    }
}
