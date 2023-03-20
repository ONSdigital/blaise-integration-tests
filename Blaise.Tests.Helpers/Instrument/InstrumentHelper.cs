using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Tests.Helpers.Configuration;
using System;
using System.Net.Http;
using System.Threading;

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

        public SurveyStatusType GetQuestionnaireStatus()
        {
            return _blaiseSurveyApi.GetSurveyStatus(BlaiseConfigurationHelper.InstrumentName,
                BlaiseConfigurationHelper.ServerParkName);
        }

        private bool DoesSurveyExists(string instrumentName = "")
        {
            try
            {
                return _blaiseSurveyApi.SurveyExists(
                    !string.IsNullOrEmpty(instrumentName) ? instrumentName : BlaiseConfigurationHelper.InstrumentName,
                    BlaiseConfigurationHelper.ServerParkName);
            }
            catch (Nuget.Api.Contracts.Exceptions.DataNotFoundException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool CheckForErroneousSurvey(string instrumentName = "")
        {
            return _blaiseSurveyApi.GetSurveyStatus(!string.IsNullOrEmpty(instrumentName) ? instrumentName : BlaiseConfigurationHelper.InstrumentName,
                BlaiseConfigurationHelper.ServerParkName) == SurveyStatusType.Erroneous;
        }

        public void InstallInstrument(string instrumentPackage = "")
        {
            if (DoesSurveyExists(BlaiseConfigurationHelper.InstrumentName))
            {
                _blaiseSurveyApi.UninstallSurvey(BlaiseConfigurationHelper.InstrumentName,
                    BlaiseConfigurationHelper.ServerParkName);
                Thread.Sleep(
                    TimeSpan.FromSeconds(int.Parse(BlaiseConfigurationHelper.UninstallSurveyTimeOutInSeconds)));
            }

            try
            {
                _blaiseSurveyApi.InstallSurvey(
                    BlaiseConfigurationHelper.InstrumentName,
                    BlaiseConfigurationHelper.ServerParkName,
                    BlaiseConfigurationHelper.InstrumentPackage,
                    SurveyInterviewType.Cati
                );
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("Bad Request"))
            {
                throw new Exception(
                    $"Error trying to install questionnaire {BlaiseConfigurationHelper.InstrumentName}. {ex.Message}");
            }

            if (CheckForErroneousSurvey(BlaiseConfigurationHelper.InstrumentName))
            {
                throw new Exception(
                    $"Error: {BlaiseConfigurationHelper.InstrumentName} is Erroneous.  You will probably need to restart Blaise to delete it.");
            }
        }

        public void InstallInstrument(SurveyInterviewType surveyConfigurationType)
        {
            if (DoesSurveyExists(BlaiseConfigurationHelper.InstrumentName))
            {
                _blaiseSurveyApi.UninstallSurvey(BlaiseConfigurationHelper.InstrumentName,
                    BlaiseConfigurationHelper.ServerParkName);
                Thread.Sleep(int.Parse(BlaiseConfigurationHelper.UninstallSurveyTimeOutInSeconds) * 1000);
            }

            if (!DoesSurveyExists(BlaiseConfigurationHelper.InstrumentName))
            {
                _blaiseSurveyApi.InstallSurvey(BlaiseConfigurationHelper.InstrumentName,
                BlaiseConfigurationHelper.ServerParkName,
                BlaiseConfigurationHelper.InstrumentPackage,
                surveyConfigurationType);

                CheckForErroneousSurvey(BlaiseConfigurationHelper.InstrumentName);
            }
            else
            {
                //The uninstall failed
                throw new Exception($"Error trying to uninstall questionnaire {BlaiseConfigurationHelper.InstrumentName}");
            }
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

        public bool SurveyExists(string instrumentName)
        {
            try
            {
                return _blaiseSurveyApi.SurveyExists(instrumentName, BlaiseConfigurationHelper.ServerParkName);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SurveyIsActive(string instrumentName, string serverPark)
        {
            return _blaiseSurveyApi.GetSurveyStatus(instrumentName, serverPark) == SurveyStatusType.Active;
        }

        public void DeactivateSurvey(string instrumentName, string serverParkName)
        {
            _blaiseSurveyApi.DeactivateSurvey(instrumentName, serverParkName);
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
