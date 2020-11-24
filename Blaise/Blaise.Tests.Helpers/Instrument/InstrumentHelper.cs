﻿using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Tests.Helpers.Configuration;

namespace Blaise.Tests.Helpers.Instrument
{
    public class InstrumentHelper
    {
        private readonly IBlaiseSurveyApi _blaiseSurveyApi;
        private readonly BlaiseConfigurationHelper _configurationHelper;

        public InstrumentHelper()
        {
            _configurationHelper = new BlaiseConfigurationHelper();
            _blaiseSurveyApi = new BlaiseSurveyApi(_configurationHelper.BuildConnectionModel());           
        }

        public void InstallInstrument(string instrumentPath)
        {
            _blaiseSurveyApi.InstallSurvey(_configurationHelper.ServerParkName, instrumentPath);
        }

        public bool CheckInstrumentIsInstalled(string instrumentName)
        {
           return _blaiseSurveyApi.SurveyExists(instrumentName, _configurationHelper.ServerParkName);
        }
    }

}