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

        public InstrumentHelper()
        {
            _configurationHelper = new BlaiseConfigurationHelper();
            _blaiseSurveyApi = new BlaiseSurveyApi(_configurationHelper.BuildConnectionModel());           
        }

        public static InstrumentHelper CreateInstance()
        {
            return new InstrumentHelper();
        }

        public void InstallInstrument()
        {
            _blaiseSurveyApi.InstallSurvey(_configurationHelper.ServerParkName, _configurationHelper.InstrumentPackage(), SurveyInterviewType.Cati);
        }

        public void InstallInstrument(string instrumentPath, SurveyInterviewType surveyConfigurationType)
        {
            _blaiseSurveyApi.InstallSurvey(_configurationHelper.ServerParkName, instrumentPath, surveyConfigurationType);
        }

        public void SurveyHasInstalled()
        {
            SurveyHasInstalled(_configurationHelper.InstrumentName, 10);
        }

        public bool SurveyHasInstalled(string instrumentName, int timeoutInSeconds)
        {
           return SurveyExists(instrumentName, timeoutInSeconds) && 
                  SurveyIsActive(instrumentName, timeoutInSeconds);
        }

        public void UninstallSurvey()
        {
            _blaiseSurveyApi.UninstallSurvey(_configurationHelper.ServerParkName, _configurationHelper.InstrumentName);
        }

        public void UninstallSurvey(string instrumentName)
        {
            _blaiseSurveyApi.UninstallSurvey(_configurationHelper.ServerParkName, instrumentName);
        }

        public SurveyInterviewType GetSurveyInterviewType(string instrumentName)
        {
            return _blaiseSurveyApi.GetSurveyInterviewType(instrumentName, _configurationHelper.ServerParkName);
        }

        private SurveyStatusType GetSurveyStatus(string instrumentName)
        {
            return _blaiseSurveyApi.GetSurveyStatus(instrumentName, _configurationHelper.ServerParkName);
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
