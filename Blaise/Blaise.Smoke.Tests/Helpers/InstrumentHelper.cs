using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaise.Smoke.Tests.Helpers
{

    public class InstrumentHelper
    {
        private readonly IBlaiseSurveyApi _blaiseSurveyApi;
        private readonly ConfigurationHelper _configurationHelper;

        public InstrumentHelper()
        {
            _configurationHelper = new ConfigurationHelper();
            _blaiseSurveyApi = new BlaiseSurveyApi(_configurationHelper.BuildConnectionModel());           
        }

        public void InstallInstrument(string instrumentPath)
        {
            _blaiseSurveyApi.InstallSurvey(_configurationHelper.ServerParkName, instrumentPath);
        }

        public bool CheckInstrumentIsInstalled(string insturmentName)
        {
           return _blaiseSurveyApi.SurveyExists(insturmentName, _configurationHelper.ServerParkName);
        }
    }

}
