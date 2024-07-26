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

        public void CheckIfQuestionnaireIsErroneous(string instrumentName)
        {
            try
            {
                var questionnaireStatus = GetQuestionnaireStatus();

                if (questionnaireStatus == QuestionnaireStatusType.Erroneous)
                {
                    Console.WriteLine(@"
                     ______ _____  _____   ____  _   _ ______ ____  _    _  _____  
                    |  ____|  __ \|  __ \ / __ \| \ | |  ____/ __ \| |  | |/ ____|
                    | |__  | |__) | |__) | |  | |  \| | |__ | |  | | |  | | (___
                    |  __| |  _  /|  _  /| |  | | . ` |  __|| |  | | |  | |\___ \
                    | |____| | \ \| | \ \| |__| | |\  | |___| |__| | |__| |____) |
                    |______|_|  \_\_|  \_\\____/|_| \_|______\____/ \____/|_____/
                    "); 
                    Console.WriteLine($"InstrumentHelper CheckIfQuestionnaireIsErroneous: Questionnaire {instrumentName} is ERRONEOUS! Restart Blaise on mgmt VM and uninstall it via Blaise Server Manager");
                    return;
                }
                Console.WriteLine($"InstrumentHelper CheckIfQuestionnaireIsErroneous: Questionnaire {instrumentName} is not erroneous, it is in the state {questionnaireStatus}");
            }
            catch (DataNotFoundException)
            {
                Console.WriteLine($"InstrumentHelper CheckIfQuestionnaireIsErroneous: Questionnaire {instrumentName} not found");
            }
        }

        public static string GetQuestionnairePackagePath(string instrumentPath, string instrumentName)
        {
            return $"{instrumentPath}//{instrumentName}.bpkg";
        }

        public void InstallQuestionnaire()
        {
            InstallQuestionnaire(BlaiseConfigurationHelper.InstrumentName);
        }

        public void InstallQuestionnaire(string instrumentName)
        {
            Console.WriteLine($"InstrumentHelper InstallQuestionnaire: Installing questionnaire {BlaiseConfigurationHelper.InstrumentName}...");
            var instrumentPackage = GetQuestionnairePackagePath(BlaiseConfigurationHelper.InstrumentPath, instrumentName);
            _blaiseQuestionnaireApi.InstallQuestionnaire(instrumentName,
                BlaiseConfigurationHelper.ServerParkName,
                instrumentPackage,
                QuestionnaireInterviewType.Cati);
        }

        public bool SurveyHasInstalled(string instrumentName, int timeoutInSeconds)
        {
            return SurveyExists(instrumentName) &&
                   SurveyIsActive(instrumentName, timeoutInSeconds);
        }

        public bool SurveyHasUninstalled(string instrumentName, int timeoutInSeconds)
        {
            return SurveyNoLongerExists(instrumentName, timeoutInSeconds);
        }

        public void UninstallSurvey()
        {
            Console.WriteLine($"InstrumentHelper UninstallSurvey: Removing questionnaire {BlaiseConfigurationHelper.InstrumentName}...");
            _blaiseQuestionnaireApi.UninstallQuestionnaire(BlaiseConfigurationHelper.InstrumentName, BlaiseConfigurationHelper.ServerParkName);

           if (!SurveyHasUninstalled(BlaiseConfigurationHelper.InstrumentName, 180))
            {
                CheckIfQuestionnaireIsErroneous(BlaiseConfigurationHelper.InstrumentName);
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

        private bool SurveyExists(string instrumentName)
        {
            Console.WriteLine($"InstrumentHelper SurveyExists: Checking questionnaire {BlaiseConfigurationHelper.InstrumentName} has been installed...");
            const int maxAttempts = 10;
            const int waitTimeInSeconds = 10;
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                if (_blaiseQuestionnaireApi.QuestionnaireExists(instrumentName, BlaiseConfigurationHelper.ServerParkName))
                {
                    Console.WriteLine($"InstrumentHelper SurveyExists: Questionnaire {BlaiseConfigurationHelper.InstrumentName} has been installed");
                    return true;
                }
                Console.WriteLine($"InstrumentHelper SurveyExists: Attempt {attempt}/{maxAttempts}. Questionnaire not found. Waiting for {waitTimeInSeconds} seconds before next check...");
                Thread.Sleep(waitTimeInSeconds * 1000);
            }
            Console.WriteLine($"InstrumentHelper SurveyExists: Timeout reached after {maxAttempts * waitTimeInSeconds} seconds. Questionnaire {instrumentName} not found");
            return false;
        }

        private bool SurveyNoLongerExists(string instrumentName, int timeoutInSeconds)
        {
            Console.WriteLine($"InstrumentHelper SurveyNoLongerExists: Checking questionnaire {BlaiseConfigurationHelper.InstrumentName} has been removed");
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
