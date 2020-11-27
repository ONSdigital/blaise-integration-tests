using System.Threading;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Tests.Helpers.Configuration;

namespace Blaise.Tests.Helpers.Instrument
{
    public class InstrumentHelper
    {
        private readonly IBlaiseSurveyApi _blaiseSurveyApi;
        private readonly BlaiseConfigurationHelper _configurationHelper;

        private static InstrumentHelper _currentInstance;

        public InstrumentHelper()
        {
            _configurationHelper = new BlaiseConfigurationHelper();
            _blaiseSurveyApi = new BlaiseSurveyApi(_configurationHelper.BuildConnectionModel());           
        }

        public static InstrumentHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new InstrumentHelper());
        }

        public void InstallInstrument()
        {
            _blaiseSurveyApi.InstallSurvey(_configurationHelper.ServerParkName, _configurationHelper.InstrumentPackage(), SurveyInterviewType.Cati);
        }

        public void InstallInstrument(SurveyInterviewType surveyConfigurationType)
        {
            _blaiseSurveyApi.InstallSurvey(_configurationHelper.ServerParkName, _configurationHelper.InstrumentPackage(), surveyConfigurationType);
        }

        public bool SurveyHasInstalled(int timeoutInSeconds)
        {
           return SurveyExists(_configurationHelper.InstrumentName, timeoutInSeconds) && 
                  SurveyIsActive(_configurationHelper.InstrumentName, timeoutInSeconds);
        }

        public void UninstallSurvey()
        {
            _blaiseSurveyApi.UninstallSurvey(_configurationHelper.ServerParkName, _configurationHelper.InstrumentName);
        }

        public SurveyInterviewType GetSurveyInterviewType()
        {
            return _blaiseSurveyApi.GetSurveyInterviewType(_configurationHelper.InstrumentName, _configurationHelper.ServerParkName);
        }


        private bool SurveyIsActive(string instrumentName, int timeoutInSeconds)
        {
            var counter = 0;
            const int maxCount = 10;

            while (GetSurveyStatus(instrumentName) == SurveyStatusType.Installing)
            {
                Thread.Sleep(timeoutInSeconds % maxCount);

                counter++;
                if (counter == maxCount)
                {
                    return false;
                }
            }

            return GetSurveyStatus(instrumentName) == SurveyStatusType.Active;
        }

        private SurveyStatusType GetSurveyStatus(string instrumentName)
        {
            return _blaiseSurveyApi.GetSurveyStatus(instrumentName, _configurationHelper.ServerParkName);
        }

        private bool SurveyExists(string instrumentName, int timeoutInSeconds)
        {
            var counter = 0;
            const int maxCount = 10;

            while (!_blaiseSurveyApi.SurveyExists(instrumentName, _configurationHelper.ServerParkName))
            {
                Thread.Sleep(timeoutInSeconds % maxCount);
                
                counter++;
                if (counter == maxCount)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
