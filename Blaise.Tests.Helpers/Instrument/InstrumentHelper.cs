using System;
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

        private static InstrumentHelper _currentInstance;

        public InstrumentHelper()
        {
            _blaiseSurveyApi = new BlaiseSurveyApi();           
        }

        public static InstrumentHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new InstrumentHelper());
        }

        public void InstallInstrument()
        {
            _blaiseSurveyApi.InstallSurvey(BlaiseConfigurationHelper.InstrumentName, 
                BlaiseConfigurationHelper.ServerParkName,
                 BlaiseConfigurationHelper.InstrumentPackage, 
                SurveyInterviewType.Cati);
        }

        public void InstallInstrument(string instrumentPackage)
        {
            _blaiseSurveyApi.InstallSurvey(BlaiseConfigurationHelper.InstrumentName,
                BlaiseConfigurationHelper.ServerParkName,
                instrumentPackage,
                SurveyInterviewType.Cati);
        }

        public void InstallInstrument(SurveyInterviewType surveyConfigurationType)
        {
            _blaiseSurveyApi.InstallSurvey(BlaiseConfigurationHelper.InstrumentName,
                BlaiseConfigurationHelper.ServerParkName,
                BlaiseConfigurationHelper.InstrumentPackage,
                surveyConfigurationType);
        }

        public bool SurveyHasInstalled(string instrumentName, int timeoutInSeconds)
        {
           return SurveyExists(instrumentName, timeoutInSeconds) && 
                  SurveyIsActive(instrumentName, timeoutInSeconds);
        }

        public void UninstallSurvey()
        {
            _blaiseSurveyApi.UninstallSurvey(BlaiseConfigurationHelper.InstrumentName, BlaiseConfigurationHelper.ServerParkName);
        }

        public void UninstallSurvey(string instrumentName, string serverParkName)
        {
            _blaiseSurveyApi.UninstallSurvey(instrumentName, serverParkName);
        }

        public SurveyInterviewType GetSurveyInterviewType()
        {
            return _blaiseSurveyApi.GetSurveyInterviewType(BlaiseConfigurationHelper.InstrumentName, BlaiseConfigurationHelper.ServerParkName);
        }

        public bool SetSurveyAsActive(int timeoutInSeconds)
        {
            _blaiseSurveyApi.ActivateSurvey(BlaiseConfigurationHelper.InstrumentName, BlaiseConfigurationHelper.ServerParkName);
            return SurveyIsActive(BlaiseConfigurationHelper.InstrumentName, timeoutInSeconds);
        }

        public bool SetSurveyAsInactive()
        {
            _blaiseSurveyApi.DeactivateSurvey(BlaiseConfigurationHelper.InstrumentName, BlaiseConfigurationHelper.ServerParkName);
            return GetSurveyStatus(BlaiseConfigurationHelper.InstrumentName) == SurveyStatusType.Inactive;
        }

        public bool SurveyExists(string instrumentName)
        {
            return _blaiseSurveyApi.SurveyExists(instrumentName, BlaiseConfigurationHelper.ServerParkName);
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
            return _blaiseSurveyApi.GetSurveyStatus(instrumentName, BlaiseConfigurationHelper.ServerParkName);
        }

        public DateTime GetInstallDate()
        {
            var survey = _blaiseSurveyApi.GetSurvey(BlaiseConfigurationHelper.InstrumentName, BlaiseConfigurationHelper.ServerParkName);

            return survey.InstallDate;
        }

        private bool SurveyExists(string instrumentName, int timeoutInSeconds)
        {
            var counter = 0;
            const int maxCount = 10;

            while (!_blaiseSurveyApi.SurveyExists(instrumentName, BlaiseConfigurationHelper.ServerParkName))
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
