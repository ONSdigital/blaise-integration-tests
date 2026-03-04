namespace Blaise.Tests.Helpers.Tobi
{
    using System;
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Interfaces;
    using Blaise.Tests.Helpers.Configuration;

    public class DaybatchHelper
    {
        private static DaybatchHelper _currentInstance;
        private readonly IBlaiseCatiApi _blaiseCatiApi;

        public DaybatchHelper()
        {
            _blaiseCatiApi = new BlaiseCatiApi();
        }

        public static DaybatchHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new DaybatchHelper());
        }

        public void SetSurveyDay(string questionnaireName, DateTime surveyDay)
        {
            var surveydays = _blaiseCatiApi.GetSurveyDays(questionnaireName, BlaiseConfigurationHelper.ServerParkName);
            if (!surveydays.Contains(surveyDay))
            {
                _blaiseCatiApi.SetSurveyDay(questionnaireName, BlaiseConfigurationHelper.ServerParkName, surveyDay);
            }
        }

        public void CreateDaybatch(string questionnaireName, DateTime daybatchDate)
        {
            _blaiseCatiApi.CreateDayBatch(questionnaireName, BlaiseConfigurationHelper.ServerParkName, daybatchDate, true);
        }

        public void RemoveSurveyDays(string questionnaireName, DateTime surveyDay)
        {
            var surveydays = _blaiseCatiApi.GetSurveyDays(questionnaireName, BlaiseConfigurationHelper.ServerParkName);
            if (surveydays.Contains(surveyDay))
            {
                _blaiseCatiApi.RemoveSurveyDay(questionnaireName, BlaiseConfigurationHelper.ServerParkName, surveyDay);
            }
        }
    }
}
