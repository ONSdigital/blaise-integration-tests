using OpenQA.Selenium;
using System;

public interface ISurveyPageSelectors
{
    string ClearCatiDataButtonPath { get; }
    string BackupDataButtonId { get; }
    string ClearDataButton { get; }
    string ExecuteButtonPath { get; }
    string FilterButton { get; }
    string ApplyButton { get; }
    string SurveyRadioButton { get; }

    string ExpectedPageText { get; }
}
