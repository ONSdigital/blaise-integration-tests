using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using OpenQA.Selenium;
using System;

public class SurveyPageSelectors : ISurveyPageSelectors
{
    public string ClearCatiDataButtonPath => @"//*[@id='MVCGridTable_SurveysGrid']/tbody/tr/td[9]/a";
    public string BackupDataButtonId => "chkBackupAll";
    public string ClearDataButton => "chkClearAll";
    public string ExecuteButtonPath => "//input[@value='Execute']";
    public string FilterButton => "//*[contains(text(), 'Filters')]";
    public string ApplyButton => "//*[contains(text(), 'Apply')]";
    public string SurveyRadioButton => $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";

    public string ExpectedPageText => "Showing";
}
