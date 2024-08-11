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

        public QuestionnaireStatusType GetQuestionnaireStatus(string questionnaireName)
        {
            return _blaiseQuestionnaireApi.GetQuestionnaireStatus(questionnaireName, BlaiseConfigurationHelper.ServerParkName);
        }

        public static string QuestionnairePackagePath(string questionnairePath, string questionnaireName)
        {
            return $"{questionnairePath}//{questionnaireName}.bpkg";
        }

        public void InstallQuestionnaire()
        {
            string questionnaireName = BlaiseConfigurationHelper.QuestionnaireName;

            QuestionnaireStatusType status = QuestionnaireStatusType.Other;

            if (CheckQuestionnaireExists(questionnaireName))
            {
                Console.WriteLine($"Attempting to uninstall questionnaire {questionnaireName} before re-installing...");
                _blaiseQuestionnaireApi.UninstallQuestionnaire(questionnaireName, BlaiseConfigurationHelper.ServerParkName);
            }

            if (CheckQuestionnaireExists(questionnaireName))
            {
                status = GetQuestionnaireStatus(questionnaireName);
                Console.WriteLine($"Questionnaire {questionnaireName} status: {status}");
            }

            if (status == QuestionnaireStatusType.Erroneous)
            {
                Console.WriteLine(@"
                     ______ _____  _____   ____  _   _ ______ ____  _    _  _____  
                    |  ____|  __ \|  __ \ / __ \| \ | |  ____/ __ \| |  | |/ ____|
                    | |__  | |__) | |__) | |  | |  \| | |__ | |  | | |  | | (___
                    |  __| |  _  /|  _  /| |  | | . ` |  __|| |  | | |  | |\___ \
                    | |____| | \ \| | \ \| |__| | |\  | |___| |__| | |__| |____) |
                    |______|_|  \_\_|  \_\\____/|_| \_|______\____/ \____/|_____/
                    ");        
                Console.WriteLine($"Questionnaire {questionnaireName} is in an erroneous state");
                Console.WriteLine("Restart Blaise and uninstall the erroneous questionnaire via Blaise Server Manager.");
                throw new Exception($"Questionnaire {questionnaireName} is in an erroneous state");
            }

            Console.WriteLine($"Installing questionnaire {questionnaireName}...");
            string questionnairePackagePath = QuestionnairePackagePath(BlaiseConfigurationHelper.QuestionnairePath, questionnaireName);
            _blaiseQuestionnaireApi.InstallQuestionnaire(questionnaireName,
                                                        BlaiseConfigurationHelper.ServerParkName,
                                                        questionnairePackagePath,
                                                        QuestionnaireInterviewType.Cati);
            Thread.Sleep(2000);
        }

        public bool CheckQuestionnaireInstalled(string questionnaireName, int timeoutInSeconds)
        {
            return CheckQuestionnaireExists(questionnaireName, timeoutInSeconds) &&
                   CheckQuestionnaireActive(questionnaireName, timeoutInSeconds);
        }

        public void UninstallQuestionnaire()
        {

            string questionnaireName = BlaiseConfigurationHelper.QuestionnaireName;

            QuestionnaireStatusType status = QuestionnaireStatusType.Other;

            if (CheckQuestionnaireExists(questionnaireName))
            {
                status = GetQuestionnaireStatus(questionnaireName);
                Console.WriteLine($"Questionnaire {questionnaireName} status: {status}");
            }

            if (status == QuestionnaireStatusType.Erroneous)
            {
                Console.WriteLine(@"
                     ______ _____  _____   ____  _   _ ______ ____  _    _  _____  
                    |  ____|  __ \|  __ \ / __ \| \ | |  ____/ __ \| |  | |/ ____|
                    | |__  | |__) | |__) | |  | |  \| | |__ | |  | | |  | | (___
                    |  __| |  _  /|  _  /| |  | | . ` |  __|| |  | | |  | |\___ \
                    | |____| | \ \| | \ \| |__| | |\  | |___| |__| | |__| |____) |
                    |______|_|  \_\_|  \_\\____/|_| \_|______\____/ \____/|_____/
                    ");        
                Console.WriteLine($"Questionnaire {questionnaireName} is in an erroneous state");
                Console.WriteLine("Restart Blaise and uninstall the erroneous questionnaire via Blaise Server Manager.");
                throw new Exception($"Questionnaire {questionnaireName} is in an erroneous state");
            }

            Console.WriteLine($"Uninstalling questionnaire {questionnaireName}...");
            _blaiseQuestionnaireApi.UninstallQuestionnaire(questionnaireName, BlaiseConfigurationHelper.ServerParkName);
            Thread.Sleep(2000);
        }

        public QuestionnaireInterviewType GetQuestionnaireInterviewType()
        {
            return _blaiseQuestionnaireApi.GetQuestionnaireInterviewType(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
        }

        public bool CheckQuestionnaireExists(string questionnaireName)
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

        public void DeactivateQuestionnaire(string questionnaireName, string serverParkName)
        {
            _blaiseQuestionnaireApi.DeactivateQuestionnaire(questionnaireName, serverParkName);
        }

        public bool CheckQuestionnaireActive(string questionnaireName, string serverPark)
        {
            return _blaiseQuestionnaireApi.GetQuestionnaireStatus(questionnaireName, serverPark) == QuestionnaireStatusType.Active;
        }

        private bool CheckQuestionnaireActive(string questionnaireName, int timeoutInSeconds)
        {
            var counter = 0;
            const int maxCount = 10;

            while (GetQuestionnaireStatus(questionnaireName) == QuestionnaireStatusType.Installing)
            {
                Thread.Sleep((timeoutInSeconds * 1000) / maxCount);

                counter++;
                if (counter == maxCount)
                {
                    return false;
                }
            }
            return GetQuestionnaireStatus(questionnaireName) == QuestionnaireStatusType.Active;
        }

        public DateTime GetQuestionnaireInstallDate()
        {
            var questionnaire = _blaiseQuestionnaireApi.GetQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);

            return questionnaire.InstallDate;
        }

        private bool CheckQuestionnaireExists(string questionnaireName, int timeoutInSeconds)
        {
            Console.WriteLine($"Checking questionnaire {BlaiseConfigurationHelper.QuestionnaireName} exists...");
            var counter = 0;
            const int maxCount = 10;

            while (!_blaiseQuestionnaireApi.QuestionnaireExists(questionnaireName, BlaiseConfigurationHelper.ServerParkName))
            {
                Console.WriteLine($"Sleep {counter} for {timeoutInSeconds / maxCount} seconds");
                Thread.Sleep((timeoutInSeconds * 1000) / maxCount);

                counter++;
                if (counter == maxCount)
                {
                    Console.WriteLine("Timed out");
                    return false;
                }
            }
            Console.WriteLine($"Questionnaire {BlaiseConfigurationHelper.QuestionnaireName} exists");

            return true;
        }
    }
}
