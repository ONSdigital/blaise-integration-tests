using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
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

        public QuestionnaireStatusType GetQuestionnaireStatus(string questionnaireName, string serverParkName)
        {
            return _blaiseQuestionnaireApi.GetQuestionnaireStatus(questionnaireName, serverParkName);
        }

        public static string QuestionnairePackagePath(string questionnairePath, string questionnaireName)
        {
            return $"{questionnairePath}//{questionnaireName}.bpkg";
        }

        public void InstallQuestionnaire(string questionnaireName, string serverParkName, string questionnairePath)
        {
            QuestionnaireStatusType status = QuestionnaireStatusType.Other;

            if (CheckQuestionnaireExists(questionnaireName, serverParkName))
            {
                Console.WriteLine($"Attempting to uninstall questionnaire {questionnaireName} before re-installing...");
                _blaiseQuestionnaireApi.UninstallQuestionnaire(questionnaireName, serverParkName);
            }

            if (CheckQuestionnaireExists(questionnaireName, serverParkName))
            {
                status = GetQuestionnaireStatus(questionnaireName, serverParkName);
                Console.WriteLine($"Questionnaire {questionnaireName} status: {status}");
            }

            if (status == QuestionnaireStatusType.Erroneous)
            {
                HandleErroneousState(questionnaireName);
            }

            Console.WriteLine($"Installing questionnaire {questionnaireName}...");
            string questionnairePackagePath = QuestionnairePackagePath(questionnairePath, questionnaireName);
            _blaiseQuestionnaireApi.InstallQuestionnaire(questionnaireName,
                                                        serverParkName,
                                                        questionnairePackagePath,
                                                        QuestionnaireInterviewType.Cati);
            Thread.Sleep(2000);
        }

        public bool CheckQuestionnaireInstalled(string questionnaireName, string serverParkName, int timeoutInSeconds)
        {
            return CheckQuestionnaireExists(questionnaireName, serverParkName, timeoutInSeconds) &&
                   CheckQuestionnaireActive(questionnaireName, serverParkName, timeoutInSeconds);
        }

        public void UninstallQuestionnaire(string questionnaireName, string serverParkName)
        {
            QuestionnaireStatusType status = QuestionnaireStatusType.Other;

            if (CheckQuestionnaireExists(questionnaireName, serverParkName))
            {
                status = GetQuestionnaireStatus(questionnaireName, serverParkName);
                Console.WriteLine($"Questionnaire {questionnaireName} status: {status}");
            }

            if (status == QuestionnaireStatusType.Erroneous)
            {
                HandleErroneousState(questionnaireName);
            }

            Console.WriteLine($"Uninstalling questionnaire {questionnaireName}...");
            _blaiseQuestionnaireApi.UninstallQuestionnaire(questionnaireName, serverParkName);
            Thread.Sleep(2000);
        }

        public QuestionnaireInterviewType GetQuestionnaireInterviewType(string questionnaireName, string serverParkName)
        {
            return _blaiseQuestionnaireApi.GetQuestionnaireInterviewType(questionnaireName, serverParkName);
        }

        public bool CheckQuestionnaireExists(string questionnaireName, string serverParkName)
        {
            try
            {
                return _blaiseQuestionnaireApi.QuestionnaireExists(questionnaireName, serverParkName);
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

        private bool CheckQuestionnaireActive(string questionnaireName, string serverParkName, int timeoutInSeconds)
        {
            var counter = 0;
            const int maxCount = 10;

            while (GetQuestionnaireStatus(questionnaireName, serverParkName) == QuestionnaireStatusType.Installing)
            {
                Thread.Sleep((timeoutInSeconds * 1000) / maxCount);

                counter++;
                if (counter == maxCount)
                {
                    return false;
                }
            }
            return GetQuestionnaireStatus(questionnaireName, serverParkName) == QuestionnaireStatusType.Active;
        }

        public DateTime GetQuestionnaireInstallDate(string questionnaireName, string serverParkName)
        {
            var questionnaire = _blaiseQuestionnaireApi.GetQuestionnaire(questionnaireName, serverParkName);

            return questionnaire.InstallDate;
        }

        private bool CheckQuestionnaireExists(string questionnaireName, string serverParkName, int timeoutInSeconds)
        {
            Console.WriteLine($"Checking questionnaire {questionnaireName} exists...");
            var counter = 0;
            const int maxCount = 10;

            while (!_blaiseQuestionnaireApi.QuestionnaireExists(questionnaireName, serverParkName))
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
            Console.WriteLine($"Questionnaire {questionnaireName} exists");

            return true;
        }

        private void HandleErroneousState(string questionnaireName)
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
    }
}
