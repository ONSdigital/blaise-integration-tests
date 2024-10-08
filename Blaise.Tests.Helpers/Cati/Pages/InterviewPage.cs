﻿using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class InterviewPage : BasePage
    {
        private const string CaseIdPath = "//div[contains(text(), 'Case:')]";
        private const string FirstFocusId = "firstFocusable";

        public InterviewPage() : base(CatiConfigurationHelper.SchedulerUrl, "LayoutSet=CATI-Interviewer_Large")
        {
        }

        public string GetCaseIdText()
        {
            return GetElementTextByPath(CaseIdPath);
        }

        public void WaitForFirstFocusObject()
        {
            GetElementTextById(FirstFocusId);
        }
    }
}
