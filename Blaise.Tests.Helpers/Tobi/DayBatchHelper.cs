using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Tests.Helpers.Configuration;

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
            _blaiseCatiApi.SetSurveyDay(instrumentName, BlaiseConfigurationHelper.ServerParkName, surveyDay);
        }

        public void CreateDayBatch(string instrumentName, DateTime dayBatchDate)
        {
            _blaiseCatiApi.CreateDayBatch(instrumentName, BlaiseConfigurationHelper.ServerParkName, dayBatchDate);
        }

    }
}
