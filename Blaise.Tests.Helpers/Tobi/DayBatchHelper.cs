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

        public void SetSurveyDay(string questionnaireName, DateTime surveyDay)
        {
            var surveydays = _blaiseCatiApi.GetSurveyDays(questionnaireName, BlaiseConfigurationHelper.ServerParkName);
            if (!surveydays.Contains(surveyDay))
                _blaiseCatiApi.SetSurveyDay(questionnaireName, BlaiseConfigurationHelper.ServerParkName, surveyDay);
        }

        public void CreateDayBatch(string questionnaireName, DateTime dayBatchDate)
        {
            _blaiseCatiApi.CreateDayBatch(questionnaireName, BlaiseConfigurationHelper.ServerParkName, dayBatchDate, true);
        }

        public void RemoveSurveyDays(string questionnaireName, DateTime surveyDay)
        {
            var surveydays = _blaiseCatiApi.GetSurveyDays(questionnaireName, BlaiseConfigurationHelper.ServerParkName);
            if (surveydays.Contains(surveyDay))
                _blaiseCatiApi.RemoveSurveyDay(questionnaireName, BlaiseConfigurationHelper.ServerParkName, surveyDay);

        }
    }
}
