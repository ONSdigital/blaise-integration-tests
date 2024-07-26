using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Tests.Helpers.Configuration;
using System;
using System.Threading;
using Blaise.Nuget.Api.Contracts.Exceptions;

namespace Blaise.Tests.Helpers.Instrument
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
            return _blaiseQuestionnaireApi.GetQuestionnaireStatus(BlaiseConfigurationHelper.InstrumentName,
                BlaiseConfigurationHelper.ServerParkName);
        }

        public void LogQuestionnaireErrorStatus(string questionnaireName)
        {
            try
            {
                var status = GetQuestionnaireStatus();
                if (status == QuestionnaireStatusType.Erroneous)
                {
                    LogErroneous(questionnaireName);
                }
                else
                {
                    Console.WriteLine($"Questionnaire '{questionnaireName}' status: {status}");
                }
            }
            catch (DataNotFoundException)
            {
                Console.WriteLine($"Questionnaire '{questionnaireName}' does not exist");
            }
        }

        private void LogErroneous(string questionnaireName)
        {
            Console.WriteLine(@"
             ______ _____  _____   ____  _   _ ______ ____  _    _  _____  
            |  ____|  __ \|  __ \ / __ \| \ | |  ____/ __ \| |  | |/ ____|
            | |__  | |__) | |__) | |  | |  \| | |__ | |  | | |  | | (___
            |  __| |  _  /|  _  /| |  | | . ` |  __|| |  | | |  | |\___ \
            | |____| | \ \| | \ \| |__| | |\  | |___| |__| | |__| |____) |
            |______|_|  \_\_|  \_\\____/|_| \_|______\____/ \____/|_____/
            ");
            Console.WriteLine($"ERROR: Questionnaire '{questionnaireName}' is ERRONEOUS! Restart Blaise on mgmt VM and uninstall it via Blaise Server Manager");
        }

        public static string GetQuestionnairePackagePath(string instrumentPath, string questionnaireName)
        {
            return $"{instrumentPath}//{questionnaireName}.bpkg";
        }

        public void InstallQuestionnaire()
        {
            InstallQuestionnaire(BlaiseConfigurationHelper.InstrumentName);
        }

        public void InstallQuestionnaire(string questionnaireName)
        {
            Console.WriteLine($"Installing questionnaire '{questionnaireName}'...");
            var packagePath = GetQuestionnairePackagePath(BlaiseConfigurationHelper.InstrumentPath, questionnaireName);
            _blaiseQuestionnaireApi.InstallQuestionnaire(questionnaireName,
                BlaiseConfigurationHelper.ServerParkName,
                packagePath,
                QuestionnaireInterviewType.Cati);
        }

        public bool IsQuestionnaireInstalled(string questionnaireName, int timeoutInSeconds)
        {
            return DoesQuestionnaireExist(questionnaireName, timeoutInSeconds) &&
                   IsQuestionnaireActive(questionnaireName, timeoutInSeconds);
        }

        public bool IsQuestionnaireUninstalled(string questionnaireName, int timeoutInSeconds)
        {
            return WaitForQuestionnaireRemoval(questionnaireName, timeoutInSeconds);
        }

        public void UninstallQuestionnaire()
        {
            Console.WriteLine($"Removing questionnaire '{BlaiseConfigurationHelper.InstrumentName}'...");
            _blaiseQuestionnaireApi.UninstallQuestionnaire(BlaiseConfigurationHelper.InstrumentName, BlaiseConfigurationHelper.ServerParkName);

            if (!IsQuestionnaireUninstalled(BlaiseConfigurationHelper.InstrumentName, 180))
            {
                LogQuestionnaireErrorStatus(BlaiseConfigurationHelper.InstrumentName);
            }
        }

        public QuestionnaireInterviewType GetQuestionnaireInterviewType()
        {
            return _blaiseQuestionnaireApi.GetQuestionnaireInterviewType(BlaiseConfigurationHelper.InstrumentName, BlaiseConfigurationHelper.ServerParkName);
        }

        public bool DoesQuestionnaireExist(string questionnaireName)
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

        public bool IsQuestionnaireActive(string questionnaireName, string serverPark)
        {
            return _blaiseQuestionnaireApi.GetQuestionnaireStatus(questionnaireName, serverPark) == QuestionnaireStatusType.Active;
        }

        public void DeactivateQuestionnaire(string questionnaireName, string serverParkName)
        {
            _blaiseQuestionnaireApi.DeactivateQuestionnaire(questionnaireName, serverParkName);
        }

        private bool IsQuestionnaireActive(string questionnaireName, int timeoutInSeconds)
        {
            Console.WriteLine($"Checking if questionnaire '{questionnaireName}' is active...");
            return WaitForQuestionnaireStatus(questionnaireName, QuestionnaireStatusType.Active, timeoutInSeconds);
        }

        private bool WaitForQuestionnaireStatus(string questionnaireName, QuestionnaireStatusType expectedStatus, int timeoutInSeconds)
        {
            const int maxAttempts = 10;
            var interval = timeoutInSeconds * 1000 / maxAttempts;

            for (int i = 0; i < maxAttempts; i++)
            {
                var status = GetQuestionnaireStatus(questionnaireName);
                if (status == expectedStatus)
                {
                    Console.WriteLine($"Questionnaire '{questionnaireName}' is now {expectedStatus}");
                    return true;
                }
                if (status != QuestionnaireStatusType.Installing)
                {
                    Console.WriteLine($"Questionnaire '{questionnaireName}' status: {status} (expected: {expectedStatus})");
                    return false;
                }
                Console.WriteLine($"Waiting for questionnaire '{questionnaireName}' to become {expectedStatus}... (attempt {i + 1}/{maxAttempts})");
                Thread.Sleep(interval);
            }
            Console.WriteLine($"Timed out waiting for questionnaire '{questionnaireName}' to become {expectedStatus}");
            return false;
        }

        private QuestionnaireStatusType GetQuestionnaireStatus(string questionnaireName)
        {
            return _blaiseQuestionnaireApi.GetQuestionnaireStatus(questionnaireName, BlaiseConfigurationHelper.ServerParkName);
        }

        public DateTime GetInstallDate()
        {
            var questionnaire = _blaiseQuestionnaireApi.GetQuestionnaire(BlaiseConfigurationHelper.InstrumentName, BlaiseConfigurationHelper.ServerParkName);
            return questionnaire.InstallDate;
        }

        private bool DoesQuestionnaireExist(string questionnaireName, int timeoutInSeconds)
        {
            Console.WriteLine($"Checking if questionnaire '{questionnaireName}' exists...");
            return WaitForQuestionnaireExistence(questionnaireName, true, timeoutInSeconds);
        }

        private bool WaitForQuestionnaireRemoval(string questionnaireName, int timeoutInSeconds)
        {
            Console.WriteLine($"Checking if questionnaire '{questionnaireName}' has been removed...");
            return WaitForQuestionnaireExistence(questionnaireName, false, timeoutInSeconds);
        }

        private bool WaitForQuestionnaireExistence(string questionnaireName, bool shouldExist, int timeoutInSeconds)
        {
            const int maxAttempts = 10;
            var interval = timeoutInSeconds * 1000 / maxAttempts;

            for (int i = 0; i < maxAttempts; i++)
            {
                var exists = DoesQuestionnaireExist(questionnaireName);
                if (exists == shouldExist)
                {
                    Console.WriteLine($"Questionnaire '{questionnaireName}' {(shouldExist ? "exists" : "has been removed")}");
                    return true;
                }
                Console.WriteLine($"Waiting for questionnaire '{questionnaireName}' {(shouldExist ? "to exist" : "to be removed")}... (attempt {i + 1}/{maxAttempts})");
                Thread.Sleep(interval);
            }
            Console.WriteLine($"Timed out waiting for questionnaire '{questionnaireName}' {(shouldExist ? "to exist" : "to be removed")}");
            return false;
        }
    }
}