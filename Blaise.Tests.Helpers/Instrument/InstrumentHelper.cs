using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Tests.Helpers.Configuration;
using System;
using System.Threading;
using Blaise.Nuget.Api.Contracts.Exceptions;

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
            try
            {
                var questionnaireStatus = GetQuestionnaireStatus();

                if (questionnaireStatus == QuestionnaireStatusType.Erroneous)
                {
                    Console.WriteLine($"InstrumentHelper CheckIfInstrumentIsErroneous :Questionnaire {instrumentName} SEEMS to be erroneous. Check on the management node if the tests continue to fail");
                    //throw new Exception($"ERROR: The {instrumentName} questionnaire has failed with the following status: {Enum.GetName(typeof(QuestionnaireStatusType), QuestionnaireStatusType.Erroneous)}. Blaise has probably got a lock on the questionnaire files and the Blaise service will likely need to be restarted on the Blaise management VM.");
                }

                Console.WriteLine($"InstrumentHelper CheckIfInstrumentIsErroneous :Questionnaire {instrumentName} is not erroneous, it is in the state {questionnaireStatus}");
            }
            catch (DataNotFoundException)
            {
                Console.WriteLine($"InstrumentHelper CheckIfInstrumentIsErroneous :Questionnaire {instrumentName} does not exist");
            }
        }

        public void CheckForErroneousInstrument(string instrumentName)
        {
            Console.WriteLine($"InstrumentHelper CheckForErroneousInstrument: Check to see if questionnaire {BlaiseConfigurationHelper.InstrumentName} has become erroneous");

            if (SurveyExists(instrumentName))
            {
                CheckIfInstrumentIsErroneous(instrumentName);
                return;
            }

            Console.WriteLine($"InstrumentHelper CheckForErroneousInstrument: Questionnaire {BlaiseConfigurationHelper.InstrumentName} is not installed");
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
            Console.WriteLine($"InstrumentHelper InstallInstrument: Questionnaire {BlaiseConfigurationHelper.InstrumentName} is about to be installed");
            var instrumentPackage = InstrumentPackagePath(BlaiseConfigurationHelper.InstrumentPath, instrumentName);

            //CheckForErroneousInstrument(instrumentName);

            Console.WriteLine($"InstrumentHelper InstallInstrument: install Questionnaire {BlaiseConfigurationHelper.InstrumentName}");
            _blaiseQuestionnaireApi.InstallQuestionnaire(instrumentName,
                BlaiseConfigurationHelper.ServerParkName,
                instrumentPackage,
                QuestionnaireInterviewType.Cati);

            //CheckForErroneousInstrument(instrumentName);
        }

        public bool SurveyHasInstalled(string instrumentName, int timeoutInSeconds)
        {
            return SurveyExists(instrumentName, timeoutInSeconds) &&
                   SurveyIsActive(instrumentName, timeoutInSeconds);
        }

        public bool SurveyHasUninstalled(string instrumentName, int timeoutInSeconds)
        {
            return SurveyNoLongerExists(instrumentName, timeoutInSeconds);
        }

        public void UninstallSurvey()
        {
            Console.WriteLine($"InstrumentHelper UninstallSurvey: Removing questionnaire {BlaiseConfigurationHelper.InstrumentName}");
            _blaiseQuestionnaireApi.UninstallQuestionnaire(BlaiseConfigurationHelper.InstrumentName, BlaiseConfigurationHelper.ServerParkName);

           if (!SurveyHasUninstalled(BlaiseConfigurationHelper.InstrumentName, 120))
            {
                CheckIfInstrumentIsErroneous(BlaiseConfigurationHelper.InstrumentName);
            }
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
                Thread.Sleep((timeoutInSeconds * 1000) / maxCount);

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
            Console.WriteLine($"InstrumentHelper SurveyExists: Check to see if questionnaire {BlaiseConfigurationHelper.InstrumentName} has been installed");
            var counter = 0;
            const int maxCount = 10;

            while (!_blaiseQuestionnaireApi.QuestionnaireExists(instrumentName, BlaiseConfigurationHelper.ServerParkName))
            {
                Console.WriteLine($"InstrumentHelper SurveyExists: Sleep {counter} for {timeoutInSeconds / maxCount} seconds");
                Thread.Sleep((timeoutInSeconds * 1000) / maxCount);

                counter++;
                if (counter == maxCount)
                {
                    Console.WriteLine("InstrumentHelper SurveyExists: Timed out");
                    return false;
                }
            }
            Console.WriteLine($"InstrumentHelper SurveyExists: Questionnaire {BlaiseConfigurationHelper.InstrumentName} has been installed");

            return true;
        }

        private bool SurveyNoLongerExists(string instrumentName, int timeoutInSeconds)
        {
            Console.WriteLine($"InstrumentHelper SurveyNoLongerExists: Check to see if questionnaire {BlaiseConfigurationHelper.InstrumentName} has been removed");
            var counter = 0;
            const int maxCount = 10;

            while (_blaiseQuestionnaireApi.QuestionnaireExists(instrumentName, BlaiseConfigurationHelper.ServerParkName))
            {
                Console.WriteLine($"InstrumentHelper SurveyNoLongerExists: Sleep {counter} for {timeoutInSeconds / maxCount} seconds");
                Thread.Sleep((timeoutInSeconds * 1000) / maxCount);

                counter++;
                if (counter == maxCount)
                {
                    Console.WriteLine("InstrumentHelper SurveyNoLongerExists: Timed out");
                    return false;
                }
            }

            Console.WriteLine($"InstrumentHelper SurveyNoLongerExists: Questionnaire {BlaiseConfigurationHelper.InstrumentName} has been removed");
            return true;
        }
    }
}
