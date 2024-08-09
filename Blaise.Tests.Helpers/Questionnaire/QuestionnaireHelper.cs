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

        public void CheckIfQuestionnaireIsErroneous(string questionnaireName)
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
                    Console.WriteLine($"QuestionnaireHelper CheckIfQuestionnaireIsErroneous: Questionnaire {questionnaireName} is ERRONEOUS! Restart Blaise on mgmt VM and uninstall it via Blaise Server Manager");
                    return;
                }
                Console.WriteLine($"QuestionnaireHelper CheckIfQuestionnaireIsErroneous: Questionnaire {questionnaireName} is not erroneous, it is in the state {questionnaireStatus}");
            }
            catch (DataNotFoundException)
            {
                Console.WriteLine($"QuestionnaireHelper CheckIfQuestionnaireIsErroneous: Questionnaire {questionnaireName} does not exist");
            }
        }

        public static string QuestionnairePackagePath(string questionnairePath, string questionnaireName)
        {
            return $"{questionnairePath}//{questionnaireName}.bpkg";
        }

        public void InstallQuestionnaire()
        {
            InstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName);
        }

        public void InstallQuestionnaire(string questionnaireName)
        {
            QuestionnaireStatusType status;

            try
            {
                // Attempt to get the status of the questionnaire
                status = GetSurveyStatus(questionnaireName);
                Console.WriteLine($"QuestionnaireHelper InstallQuestionnaire: Questionnaire {questionnaireName} status is {status}");
            }
            catch (Exception ex)
            {
                // Check if the exception indicates that no questionnaire was found
                if (ex.Message.Contains("No questionnaire found"))
                {
                    Console.WriteLine($"QuestionnaireHelper InstallQuestionnaire: No questionnaire found for {questionnaireName}. Proceeding with installation.");
                }
                else
                {
                    // Re-throw the exception if it's not related to a missing questionnaire
                    throw;
                }
            }

            if (status == QuestionnaireStatusType.Erroneous)
            {
                Console.WriteLine($"QuestionnaireHelper InstallQuestionnaire: Questionnaire {questionnaireName} is in erroneous state.");
                throw new Exception($"Questionnaire '{questionnaireName}' cannot be installed because it is already installed and in an erroneous state.");
            }

            // Proceed with installation if the questionnaire is not erroneous
            Console.WriteLine($"QuestionnaireHelper InstallQuestionnaire: Installing questionnaire {questionnaireName}...");
            string questionnairePackagePath = QuestionnairePackagePath(BlaiseConfigurationHelper.QuestionnairePath, questionnaireName);
            _blaiseQuestionnaireApi.InstallQuestionnaire(questionnaireName,
                                                        BlaiseConfigurationHelper.ServerParkName,
                                                        questionnairePackagePath,
                                                        QuestionnaireInterviewType.Cati);
        }

        public bool SurveyHasInstalled(string questionnaireName, int timeoutInSeconds)
        {
            return SurveyExists(questionnaireName, timeoutInSeconds) &&
                   SurveyIsActive(questionnaireName, timeoutInSeconds);
        }

        public bool SurveyHasUninstalled(string questionnaireName, int timeoutInSeconds)
        {
            return SurveyNoLongerExists(questionnaireName, timeoutInSeconds);
        }

        public void UninstallSurvey()
        {
            Console.WriteLine($"QuestionnaireHelper UninstallSurvey: Removing questionnaire {BlaiseConfigurationHelper.QuestionnaireName}...");
            _blaiseQuestionnaireApi.UninstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);

           if (!SurveyHasUninstalled(BlaiseConfigurationHelper.QuestionnaireName, 180))
            {
                CheckIfQuestionnaireIsErroneous(BlaiseConfigurationHelper.QuestionnaireName);
            }
        }

        public QuestionnaireInterviewType GetSurveyInterviewType()
        {
            return _blaiseQuestionnaireApi.GetQuestionnaireInterviewType(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
        }

        public bool SurveyExists(string questionnaireName)
        {
            try
            {
                return _blaiseQuestionnaireApi.QuestionnaireExists(questionnaireName, BlaiseConfigurationHelper.ServerParkName);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SurveyIsActive(string questionnaireName, string serverPark)
        {
            return _blaiseQuestionnaireApi.GetQuestionnaireStatus(questionnaireName, serverPark) == QuestionnaireStatusType.Active;
        }

        public void DeactivateSurvey(string questionnaireName, string serverParkName)
        {
            _blaiseQuestionnaireApi.DeactivateQuestionnaire(questionnaireName, serverParkName);
        }

        private bool SurveyIsActive(string questionnaireName, int timeoutInSeconds)
        {
            var counter = 0;
            const int maxCount = 10;

            while (GetSurveyStatus(questionnaireName) == QuestionnaireStatusType.Installing)
            {
                Thread.Sleep((timeoutInSeconds * 1000) / maxCount);

                counter++;
                if (counter == maxCount)
                {
                    return false;
                }
            }
            return GetSurveyStatus(questionnaireName) == QuestionnaireStatusType.Active;
        }

        private QuestionnaireStatusType GetSurveyStatus(string questionnaireName)
        {
            return _blaiseQuestionnaireApi.GetQuestionnaireStatus(questionnaireName, BlaiseConfigurationHelper.ServerParkName);
        }

        public DateTime GetInstallDate()
        {
            var survey = _blaiseQuestionnaireApi.GetQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);

            return survey.InstallDate;
        }

        private bool SurveyExists(string questionnaireName, int timeoutInSeconds)
        {
            Console.WriteLine($"QuestionnaireHelper SurveyExists: Checking questionnaire {BlaiseConfigurationHelper.QuestionnaireName} has been installed...");
            var counter = 0;
            const int maxCount = 10;

            while (!_blaiseQuestionnaireApi.QuestionnaireExists(questionnaireName, BlaiseConfigurationHelper.ServerParkName))
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

        private bool SurveyNoLongerExists(string questionnaireName, int timeoutInSeconds)
        {
            Console.WriteLine($"QuestionnaireHelper SurveyNoLongerExists: Checking questionnaire {BlaiseConfigurationHelper.QuestionnaireName} has been removed");
            var counter = 0;
            const int maxCount = 10;

            while (_blaiseQuestionnaireApi.QuestionnaireExists(questionnaireName, BlaiseConfigurationHelper.ServerParkName))
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
