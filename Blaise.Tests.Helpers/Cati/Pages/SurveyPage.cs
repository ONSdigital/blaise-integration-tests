using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using OpenQA.Selenium;
using System;
using System.Configuration;
using System.Threading;

public class SurveyPage : BasePage
{
    private readonly ISurveyPageSelectors _selectors;

    public SurveyPage()
        : base(CatiConfigurationHelper.SurveyUrl)
    {
        var isNewVersion = Convert.ToBoolean(ConfigurationManager.AppSettings["ENV_IS_BLAIS16"]);

        if (isNewVersion)
        {
            _selectors = new NewSurveyPageSelectors();
        }
        else
        {
            _selectors = new SurveyPageSelectors();
        }
    }

    protected override Func<IWebDriver, bool> PageHasLoaded()
    {
        return BodyContainsText(_selectors.ExpectedPageText);
    }

    public void ClearDayBatchEntries()
    {
        Thread.Sleep(2000);

        ClickButtonByXPath(_selectors.ClearCatiDataButtonPath);
        ClickButtonByIdOrXPath(_selectors.BackupDataButtonId);
        ClickButtonByIdOrXPath(_selectors.ClearDataButton);
        ClickButtonByXPath(_selectors.ExecuteButtonPath);
    }

    private void ClickButtonByIdOrXPath(string selector)
    {
        if (selector.StartsWith("//"))
        {
            ClickButtonByXPath(selector);
        }
        else
        {
            ClickButtonById(selector);
        }
    }

    public void ApplyFilter()
    {
        ClickButtonByXPath(_selectors.FilterButton);
        var filterButtonText = GetElementTextByPath(_selectors.FilterButton);

        if (filterButtonText != "Filters (active)")
        {
            ClickButtonByXPath(_selectors.SurveyRadioButton);
            ClickButtonByXPath(_selectors.ApplyButton);
        }
    }
}
