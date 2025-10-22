using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using OpenQA.Selenium;
using System;

public class NewSurveyPageSelectors : ISurveyPageSelectors
{
    public string ClearCatiDataButtonPath => "//*[contains(text(), 'Clear')]";
    public string BackupDataButtonId => "qa_backupdata_0";
    public string ClearDataButton => "//label[@for='qa_clear_all']";
    public string ExecuteButtonPath => "//button[@id='qa_btn_submit']";
    public string FilterButton => "//*[contains(text(), 'Filters')]";
    public string ApplyButton => "//*[contains(text(), 'Apply')]";
    public string SurveyRadioButton => $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";

    public string ExpectedPageText => "Surveys";
}
