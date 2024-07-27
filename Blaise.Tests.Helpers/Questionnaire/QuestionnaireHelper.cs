using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Tests.Helpers.Configuration;
using System;
using System.Threading;
using Blaise.Nuget.Api.Contracts.Exceptions;

namespace Blaise.Tests.Helpers.Questionnaire
{
    public class QuestionnaireHelper
    {
        private readonly IBlaiseQuestionnaireApi _blaiseQuestionnaireApi;

        private static QuestionnaireHelper _currentInstance;

        public QuestionnaireHelper()
        {
            _blaiseQuestionnaireApi = new BlaiseQuestionnaireApi();
        }

        public static QuestionnaireHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new QuestionnaireHelper());
        }

        public QuestionnaireStatusType GetQuestionnaireStatus()
        {
            return _blaiseQuestionnaireApi.GetQuestionnaireStatus(BlaiseConfigurationHelper.QuestionnaireName,
                BlaiseConfigurationHelper.ServerParkName);
        }

        public void CheckIfInstrumentIsErroneous(string instrumentName)
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
                    Console.WriteLine($"QuestionnaireHelper CheckIfInstrumentIsErroneous: Questionnaire {instrumentName} is ERRONEOUS! Restart Blaise on mgmt VM and uninstall it via Blaise Server Manager");
                    return;
                }
                Console.WriteLine($"QuestionnaireHelper CheckIfInstrumentIsErroneous: Questionnaire {instrumentName} is not erroneous, it is in the state {questionnaireStatus}");
            }
            catch (DataNotFoundException)
            {
                Console.WriteLine($"QuestionnaireHelper CheckIfInstrumentIsErroneous: Questionnaire {instrumentName} does not exist");
            }
        }

        public static string InstrumentPackagePath(string instrumentPath, string instrumentName)
        {
            return $"{instrumentPath}//{instrumentName}.bpkg";
        }

        public void InstallQuestionnaire()
        {
            InstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName);
        }

        public void InstallQuestionnaire(string instrumentName)
        {
            Console.WriteLine($"QuestionnaireHelper InstallQuestionnaire: Installing questionnaire {BlaiseConfigurationHelper.QuestionnaireName}...");
            var instrumentPackage = InstrumentPackagePath(BlaiseConfigurationHelper.InstrumentPath, instrumentName);
            _blaiseQuestionnaireApi.InstallQuestionnaire(instrumentName,
                BlaiseConfigurationHelper.ServerParkName,
                instrumentPackage,
                QuestionnaireInterviewType.Cati);
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
            Console.WriteLine($"QuestionnaireHelper UninstallSurvey: Removing questionnaire {BlaiseConfigurationHelper.QuestionnaireName}...");
            _blaiseQuestionnaireApi.UninstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);

           if (!SurveyHasUninstalled(BlaiseConfigurationHelper.QuestionnaireName, 180))
            {
                CheckIfInstrumentIsErroneous(BlaiseConfigurationHelper.QuestionnaireName);
            }
        }

        public QuestionnaireInterviewType GetSurveyInterviewType()
        {
            return _blaiseQuestionnaireApi.GetQuestionnaireInterviewType(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
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
            var survey = _blaiseQuestionnaireApi.GetQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);

            return survey.InstallDate;
        }

        private bool SurveyExists(string instrumentName, int timeoutInSeconds)
        {
            Console.WriteLine($"QuestionnaireHelper SurveyExists: Checking questionnaire {BlaiseConfigurationHelper.QuestionnaireName} has been installed...");
            var counter = 0;
            const int maxCount = 10;

            while (!_blaiseQuestionnaireApi.QuestionnaireExists(instrumentName, BlaiseConfigurationHelper.ServerParkName))
            {
                Console.WriteLine($"QuestionnaireHelper SurveyExists: Sleep {counter} for {timeoutInSeconds / maxCount} seconds");
                Thread.Sleep((timeoutInSeconds * 1000) / maxCount);

                counter++;
                if (counter == maxCount)
                {
                    Console.WriteLine("QuestionnaireHelper SurveyExists: Timed out");
                    return false;
                }
            }
            Console.WriteLine($"QuestionnaireHelper SurveyExists: Questionnaire {BlaiseConfigurationHelper.QuestionnaireName} has been installed");

            return true;
        }

        private bool SurveyNoLongerExists(string instrumentName, int timeoutInSeconds)
        {
            Console.WriteLine($"QuestionnaireHelper SurveyNoLongerExists: Checking questionnaire {BlaiseConfigurationHelper.QuestionnaireName} has been removed");
            var counter = 0;
            const int maxCount = 10;

            while (_blaiseQuestionnaireApi.QuestionnaireExists(instrumentName, BlaiseConfigurationHelper.ServerParkName))
            {
                Console.WriteLine($"QuestionnaireHelper SurveyNoLongerExists: Sleep {counter} for {timeoutInSeconds / maxCount} seconds");
                Thread.Sleep((timeoutInSeconds * 1000) / maxCount);

                counter++;
                if (counter == maxCount)
                {
                    Console.WriteLine("QuestionnaireHelper SurveyNoLongerExists: Timed out");
                    return false;
                }
            }

            Console.WriteLine($"QuestionnaireHelper SurveyNoLongerExists: Questionnaire {BlaiseConfigurationHelper.QuestionnaireName} has been removed");
            return true;
        }
    }
}
