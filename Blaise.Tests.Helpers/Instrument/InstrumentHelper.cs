using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Tests.Helpers.Configuration;
using System;
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

        private bool DoesSurveyExists(string instrumentName)
        {
            try
            {
                return _blaiseSurveyApi.SurveyExists(instrumentName, BlaiseConfigurationHelper.ServerParkName);
            }
            catch (Nuget.Api.Contracts.Exceptions.DataNotFoundException)
            {
                return false;
            }
        }

        public void CheckIfInstrumentIsErroneous(string instrumentName)
        {
            if (_blaiseSurveyApi.GetSurveyStatus(instrumentName, BlaiseConfigurationHelper.ServerParkName) == SurveyStatusType.Erroneous)
            {
                throw new Exception($"ERROR: The {instrumentName} questionnaire has failed with the following status: {Enum.GetName(typeof(SurveyStatusType), SurveyStatusType.Erroneous)}. Blaise has probably got a lock on the questionnaire files and the Blaise service will likely need to be restarted on the Blaise management VM.");
            }
        }

        public void CheckForErroneousInstrument(string instrumentName)
        {
            if (DoesSurveyExists(instrumentName))
            {
                CheckIfInstrumentIsErroneous(instrumentName);
            }
        }

        public static string InstrumentPackagePath(string instrumentPath, string instrumentName)
        {
            return $"{instrumentPath}//{instrumentName}.bpkg";
        }

        public void InstallInstrument()
        {
            InstallInstrument(BlaiseConfigurationHelper.InstrumentName);
        }

        public void InstallInstrument(string instrumentName)
        {
            var instrumentPackage = InstrumentPackagePath(BlaiseConfigurationHelper.InstrumentPath, instrumentName);

            if (DoesSurveyExists(instrumentName))
            {
                _blaiseSurveyApi.UninstallSurvey(instrumentName,
                    BlaiseConfigurationHelper.ServerParkName);
                Thread.Sleep(int.Parse(BlaiseConfigurationHelper.UninstallSurveyTimeOutInSeconds) * 1000);
            }

            if (DoesSurveyExists(instrumentName))
            {
                CheckIfInstrumentIsErroneous(instrumentName);
            }

            _blaiseSurveyApi.InstallSurvey(instrumentName,
                BlaiseConfigurationHelper.ServerParkName,
                instrumentPackage,
                SurveyInterviewType.Cati);

            CheckIfInstrumentIsErroneous(instrumentName);
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
