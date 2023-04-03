using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Tests.Helpers.Configuration;
using System;

namespace Blaise.Tests.Helpers.Tobi
{
    public class DayBatchHelper
    {
        private readonly IBlaiseCatiApi _blaiseCatiApi;
        private static DayBatchHelper _currentInstance;

        public DayBatchHelper()
        {
            _blaiseCatiApi = new BlaiseCatiApi();
        }

        public static DayBatchHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new DayBatchHelper());
        }

        public void SetSurveyDay(string instrumentName, DateTime surveyDay)
        {
            var surveydays = _blaiseCatiApi.GetSurveyDays(instrumentName, BlaiseConfigurationHelper.ServerParkName);
            if (!surveydays.Contains(surveyDay))
                _blaiseCatiApi.SetSurveyDay(instrumentName, BlaiseConfigurationHelper.ServerParkName, surveyDay);
        }

        public void CreateDayBatch(string instrumentName, DateTime dayBatchDate)
        {
            _blaiseCatiApi.CreateDayBatch(instrumentName, BlaiseConfigurationHelper.ServerParkName, dayBatchDate);
        }

        public void RemoveSurveyDays(string instrumentName, DateTime surveyDay)
        {
            var surveydays = _blaiseCatiApi.GetSurveyDays(instrumentName, BlaiseConfigurationHelper.ServerParkName);
            if (surveydays.Contains(surveyDay))
                _blaiseCatiApi.RemoveSurveyDay(instrumentName, BlaiseConfigurationHelper.ServerParkName, surveyDay);

        }
    }
}
