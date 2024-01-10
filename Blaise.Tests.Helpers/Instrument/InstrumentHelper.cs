using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Tests.Helpers.Configuration;
using System;
using System.Threading;

namespace Blaise.Tests.Helpers.Instrument
{
    public class InstrumentHelper
    {
        private readonly IBlaiseQuestionnaireApi _blaiseQuestionnaireApi;

        private static InstrumentHelper _currentInstance;

        public InstrumentHelper()
        {
            _blaiseQuestionnaireApi = new BlaiseQuestionnaireApi();
        }

        public static InstrumentHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new InstrumentHelper());
        }

        public QuestionnaireStatusType GetQuestionnaireStatus()
        {
            return _blaiseQuestionnaireApi.GetQuestionnaireStatus(BlaiseConfigurationHelper.InstrumentName,
                BlaiseConfigurationHelper.ServerParkName);
        }

        public void CheckIfInstrumentIsErroneous(string instrumentName)
        {
            if (_blaiseQuestionnaireApi.GetQuestionnaireStatus(instrumentName, BlaiseConfigurationHelper.ServerParkName) == QuestionnaireStatusType.Erroneous)
            {
                throw new Exception($"ERROR: The {instrumentName} questionnaire has failed with the following status: {Enum.GetName(typeof(QuestionnaireStatusType), QuestionnaireStatusType.Erroneous)}. Blaise has probably got a lock on the questionnaire files and the Blaise service will likely need to be restarted on the Blaise management VM.");
            }
        }

        public void CheckForErroneousInstrument(string instrumentName)
        {
            if (SurveyExists(instrumentName))
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

            //if (DoesSurveyExists(instrumentName))
            //{
            //    _blaiseQuestionnaireApi.UninstallQuestionnaire(instrumentName,
            //        BlaiseConfigurationHelper.ServerParkName);
            //    Thread.Sleep(int.Parse(BlaiseConfigurationHelper.UninstallSurveyTimeOutInSeconds) * 1000);
            //}

            //if (DoesSurveyExists(instrumentName))
            //{
            //    CheckIfInstrumentIsErroneous(instrumentName);
            //}

            _blaiseQuestionnaireApi.InstallQuestionnaire(instrumentName,
                BlaiseConfigurationHelper.ServerParkName,
                instrumentPackage,
                QuestionnaireInterviewType.Cati);

            CheckIfInstrumentIsErroneous(instrumentName);
        }

        public bool SurveyHasInstalled(string instrumentName, int timeoutInSeconds)
        {
            return SurveyExists(instrumentName, timeoutInSeconds) &&
                   SurveyIsActive(instrumentName, timeoutInSeconds);
        }

        public void UninstallSurvey()
        {
            _blaiseQuestionnaireApi.UninstallQuestionnaire(BlaiseConfigurationHelper.InstrumentName, BlaiseConfigurationHelper.ServerParkName);
        }

        public void UninstallSurvey(string instrumentName, string serverParkName)
        {
            _blaiseQuestionnaireApi.UninstallQuestionnaire(instrumentName, serverParkName);
        }

        public QuestionnaireInterviewType GetSurveyInterviewType()
        {
            return _blaiseQuestionnaireApi.GetQuestionnaireInterviewType(BlaiseConfigurationHelper.InstrumentName, BlaiseConfigurationHelper.ServerParkName);
        }

        public bool SurveyExists(string instrumentName)
        {
            try
            {
                return _blaiseQuestionnaireApi.QuestionnaireExists(instrumentName, BlaiseConfigurationHelper.ServerParkName);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SurveyIsActive(string instrumentName, string serverPark)
        {
            return _blaiseQuestionnaireApi.GetQuestionnaireStatus(instrumentName, serverPark) == QuestionnaireStatusType.Active;
        }

        public void DeactivateSurvey(string instrumentName, string serverParkName)
        {
            _blaiseQuestionnaireApi.DeactivateQuestionnaire(instrumentName, serverParkName);
        }

        private bool SurveyIsActive(string instrumentName, int timeoutInSeconds)
        {
            var counter = 0;
            const int maxCount = 10;

            while (GetSurveyStatus(instrumentName) == QuestionnaireStatusType.Installing)
            {
                Thread.Sleep(timeoutInSeconds % maxCount);

                counter++;
                if (counter == maxCount)
                {
                    return false;
                }
            }
            return GetSurveyStatus(instrumentName) == QuestionnaireStatusType.Active;
        }

        private QuestionnaireStatusType GetSurveyStatus(string instrumentName)
        {
            return _blaiseQuestionnaireApi.GetQuestionnaireStatus(instrumentName, BlaiseConfigurationHelper.ServerParkName);
        }

        public DateTime GetInstallDate()
        {
            var survey = _blaiseQuestionnaireApi.GetQuestionnaire(BlaiseConfigurationHelper.InstrumentName, BlaiseConfigurationHelper.ServerParkName);

            return survey.InstallDate;
        }

        private bool SurveyExists(string instrumentName, int timeoutInSeconds)
        {
            var counter = 0;
            const int maxCount = 10;

            while (!_blaiseQuestionnaireApi.QuestionnaireExists(instrumentName, BlaiseConfigurationHelper.ServerParkName))
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
