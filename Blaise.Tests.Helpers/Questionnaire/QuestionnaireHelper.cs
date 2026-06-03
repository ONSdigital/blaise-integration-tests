namespace Blaise.Tests.Helpers.Questionnaire
{
    using System;
    using System.Threading;
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Exceptions;
    using Blaise.Nuget.Api.Contracts.Interfaces;
    using Blaise.Nuget.Api.Contracts.Models;
    using StatNeth.Blaise.API.ServerManager;

    public class QuestionnaireHelper
    {
        private const int DefaultStatusTimeoutSeconds = 30;
        private const int PollingIntervalMilliseconds = 1000;

        private static QuestionnaireHelper _currentInstance;

        private readonly IBlaiseQuestionnaireApi _blaiseQuestionnaireApi;

        public QuestionnaireHelper()
        {
            _blaiseQuestionnaireApi = new BlaiseQuestionnaireApi();
        }

        public static QuestionnaireHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new QuestionnaireHelper());
        }

        public static string QuestionnairePackagePath(string questionnairePath, string questionnaireName)
        {
            return $"{questionnairePath}//{questionnaireName}.bpkg";
        }

        public QuestionnaireStatusType GetQuestionnaireStatus(string questionnaireName, string serverParkName)
        {
            return GetQuestionnaireStatusSafe(questionnaireName, serverParkName);
        }

        public void InstallQuestionnaire(string questionnaireName, string serverParkName, string questionnairePath, InstallOptions installOptions)
        {
            EnsureQuestionnaireNotInBlockedState(questionnaireName, serverParkName, "before install");

            if (CheckQuestionnaireExists(questionnaireName, serverParkName))
            {
                Console.WriteLine($"Questionnaire {questionnaireName} already exists. Uninstalling before re-installing...");
                UninstallQuestionnaire(questionnaireName, serverParkName);
                WaitForQuestionnaireToDisappear(questionnaireName, serverParkName, DefaultStatusTimeoutSeconds);
            }

            Console.WriteLine($"Installing questionnaire {questionnaireName} on server park {serverParkName}...");
            string questionnairePackagePath = QuestionnairePackagePath(questionnairePath, questionnaireName);
            Console.WriteLine($"Questionnaire package path: {questionnairePackagePath}");
            _blaiseQuestionnaireApi.InstallQuestionnaire(
                questionnaireName,
                serverParkName,
                questionnairePackagePath,
                installOptions);

            WaitForQuestionnaireStatus(
                questionnaireName,
                serverParkName,
                QuestionnaireStatusType.Active,
                DefaultStatusTimeoutSeconds);
        }

        public bool CheckQuestionnaireInstalled(string questionnaireName, string serverParkName, int timeoutInSeconds)
        {
            return CheckQuestionnaireExists(questionnaireName, serverParkName, timeoutInSeconds) &&
                    CheckQuestionnaireActive(questionnaireName, serverParkName, timeoutInSeconds);
        }

        public void UninstallQuestionnaire(string questionnaireName, string serverParkName)
        {
            if (!CheckQuestionnaireExists(questionnaireName, serverParkName))
            {
                Console.WriteLine($"Questionnaire {questionnaireName} does not exist on server park {serverParkName}. Nothing to uninstall.");
                return;
            }

            EnsureQuestionnaireNotInBlockedState(questionnaireName, serverParkName, "before uninstall");

            Console.WriteLine($"Uninstalling questionnaire {questionnaireName} from server park {serverParkName}...");
            _blaiseQuestionnaireApi.UninstallQuestionnaire(questionnaireName, serverParkName);
            WaitForQuestionnaireToDisappear(questionnaireName, serverParkName, DefaultStatusTimeoutSeconds);
        }

        public QuestionnaireConfigurationModel GetQuestionnaireConfigurationModel(string questionnaireName, string serverParkName)
        {
            return _blaiseQuestionnaireApi.GetQuestionnaireConfigurationModel(questionnaireName, serverParkName);
        }

        public bool CheckQuestionnaireExists(string questionnaireName, string serverParkName)
        {
            try
            {
                return _blaiseQuestionnaireApi.QuestionnaireExists(questionnaireName, serverParkName);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Failed to check if questionnaire {questionnaireName} exists on server park {serverParkName}. Error: {ex.Message}",
                    ex);
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

        public DateTime GetQuestionnaireInstallDate(string questionnaireName, string serverParkName)
        {
            var questionnaire = _blaiseQuestionnaireApi.GetQuestionnaire(questionnaireName, serverParkName);

            return questionnaire.InstallDate;
        }

        public void EnsureQuestionnaireReadyForTest(string questionnaireName, string serverParkName)
        {
            if (!CheckQuestionnaireExists(questionnaireName, serverParkName))
            {
                return;
            }

            var status = GetQuestionnaireStatusSafe(questionnaireName, serverParkName);

            if (status == QuestionnaireStatusType.Active)
            {
                return;
            }

            if (status == QuestionnaireStatusType.Installing)
            {
                HandleInstallingState(questionnaireName, serverParkName);
            }

            if (status == QuestionnaireStatusType.Erroneous)
            {
                HandleErroneousState(questionnaireName, serverParkName);
            }

            Console.WriteLine($"Questionnaire {questionnaireName} is in {status} status. Uninstalling to get clean state...");
            UninstallQuestionnaire(questionnaireName, serverParkName);
        }

        private bool CheckQuestionnaireActive(string questionnaireName, string serverParkName, int timeoutInSeconds)
        {
            var counter = 0;
            const int MaxCount = 10;

            var status = GetQuestionnaireStatusSafe(questionnaireName, serverParkName);

            while (status == QuestionnaireStatusType.Installing)
            {
                Thread.Sleep((timeoutInSeconds * 1000) / MaxCount);

                counter++;
                if (counter == MaxCount)
                {
                    return false;
                }

                status = GetQuestionnaireStatusSafe(questionnaireName, serverParkName);
            }

            if (status == QuestionnaireStatusType.Erroneous)
            {
                HandleErroneousState(questionnaireName, serverParkName);
            }

            return status == QuestionnaireStatusType.Active;
        }

        private bool CheckQuestionnaireExists(string questionnaireName, string serverParkName, int timeoutInSeconds)
        {
            Console.WriteLine($"Checking questionnaire {questionnaireName} exists...");
            var counter = 0;
            const int MaxCount = 10;

            while (!QuestionnaireExistsSafe(questionnaireName, serverParkName))
            {
                Console.WriteLine($"Sleep {counter} for {timeoutInSeconds / MaxCount} seconds...");
                Thread.Sleep((timeoutInSeconds * 1000) / MaxCount);

                counter++;
                if (counter == MaxCount)
                {
                    Console.WriteLine($"Timed out checking if questionnaire {questionnaireName} exists");
                    return false;
                }
            }

            Console.WriteLine($"Questionnaire {questionnaireName} exists");

            return true;
        }

        private void HandleErroneousState(string questionnaireName, string serverParkName)
        {
            var installDate = TryGetInstallDate(questionnaireName, serverParkName);
            string erroneousAsciiArt = @"
                         ______ _____  _____   ____  _   _ ______ ____  _    _  _____
                        |  ____|  __ \|  __ \ / __ \| \ | |  ____/ __ \| |  | |/ ____|
                        | |__  | |__) | |__) | |  | |  \| | |__ | |  | | |  | | (___
                        |  __| |  _  /|  _  /| |  | | . ` |  __|| |  | | |  | |\___ \
                        | |____| | \ \| | \ \| |__| | |\  | |___| |__| | |__| |____) |
                        |______|_|  \_\_|  \_\\____/|_| \_|______\____/ \____/|_____/
                ";
            string erroneousExceptionMessage = $"{erroneousAsciiArt}\n" +
                $"Questionnaire {questionnaireName} on server park {serverParkName} is erroneous!\n" +
                $"Install date: {installDate}\n" +
                "Restart Blaise and uninstall the erroneous questionnaire via Blaise Server Manager";
            throw new Exception(erroneousExceptionMessage);
        }

        private void HandleInstallingState(string questionnaireName, string serverParkName)
        {
            var installDate = TryGetInstallDate(questionnaireName, serverParkName);
            string installingExceptionMessage = $"Questionnaire {questionnaireName} on server park {serverParkName} is stuck in Installing state\n" +
                $"Install date: {installDate}\n" +
                "Restart Blaise and uninstall the questionnaire via Blaise Server Manager";
            throw new Exception(installingExceptionMessage);
        }

        private void EnsureQuestionnaireNotInBlockedState(string questionnaireName, string serverParkName, string context)
        {
            if (!CheckQuestionnaireExists(questionnaireName, serverParkName))
            {
                return;
            }

            QuestionnaireStatusType status;
            try
            {
                status = GetQuestionnaireStatusSafe(questionnaireName, serverParkName);
            }
            catch (Exception ex) when (ex is DataNotFoundException || ex.InnerException is DataNotFoundException)
            {
                Console.WriteLine($"Questionnaire {questionnaireName} disappeared between exists check and status check ({context}). Nothing to do.");
                return;
            }
            Console.WriteLine($"Questionnaire {questionnaireName} status {status} ({context}).");

            if (status == QuestionnaireStatusType.Installing)
            {
                HandleInstallingState(questionnaireName, serverParkName);
            }

            if (status == QuestionnaireStatusType.Erroneous)
            {
                HandleErroneousState(questionnaireName, serverParkName);
            }
        }

        private void WaitForQuestionnaireStatus(string questionnaireName, string serverParkName, QuestionnaireStatusType expectedStatus, int timeoutInSeconds)
        {
            var start = DateTime.UtcNow;
            QuestionnaireStatusType lastStatus = QuestionnaireStatusType.Other;

            while (DateTime.UtcNow - start < TimeSpan.FromSeconds(timeoutInSeconds))
            {
                if (!QuestionnaireExistsSafe(questionnaireName, serverParkName))
                {
                    lastStatus = QuestionnaireStatusType.Other;
                }
                else
                {
                    lastStatus = GetQuestionnaireStatusSafe(questionnaireName, serverParkName);
                }

                Console.WriteLine($"Questionnaire {questionnaireName} status: {lastStatus} (waiting for {expectedStatus})");

                if (lastStatus == QuestionnaireStatusType.Erroneous)
                {
                    HandleErroneousState(questionnaireName, serverParkName);
                }

                if (lastStatus == expectedStatus)
                {
                    return;
                }

                Thread.Sleep(PollingIntervalMilliseconds);
            }

            throw new Exception(
                $"Timed out after {timeoutInSeconds}s waiting for questionnaire {questionnaireName} " +
                $"to reach status {expectedStatus}. Last status: {lastStatus}.");
        }

        private void WaitForQuestionnaireToDisappear(string questionnaireName, string serverParkName, int timeoutInSeconds)
        {
            var start = DateTime.UtcNow;
            QuestionnaireStatusType lastStatus = QuestionnaireStatusType.Other;

            while (DateTime.UtcNow - start < TimeSpan.FromSeconds(timeoutInSeconds))
            {
                if (!QuestionnaireExistsSafe(questionnaireName, serverParkName))
                {
                    Console.WriteLine($"Questionnaire {questionnaireName} has been removed from server park {serverParkName}.");
                    return;
                }

                try
                {
                    lastStatus = GetQuestionnaireStatusSafe(questionnaireName, serverParkName);
                }
                catch (Exception ex) when (ex is DataNotFoundException || ex.InnerException is DataNotFoundException)
                {
                    Console.WriteLine($"Questionnaire {questionnaireName} disappeared between exists check and status check. Treating as removed.");
                    return;
                }
                Console.WriteLine($"Questionnaire {questionnaireName} still exists with status {lastStatus}.");

                if (lastStatus == QuestionnaireStatusType.Erroneous)
                {
                    HandleErroneousState(questionnaireName, serverParkName);
                }

                if (lastStatus == QuestionnaireStatusType.Installing)
                {
                    HandleInstallingState(questionnaireName, serverParkName);
                }

                Thread.Sleep(PollingIntervalMilliseconds);
            }

            throw new Exception(
                $"Timed out after {timeoutInSeconds}s waiting for questionnaire {questionnaireName} " +
                $"to be removed. Last status: {lastStatus}.");
        }

        private bool QuestionnaireExistsSafe(string questionnaireName, string serverParkName)
        {
            try
            {
                return _blaiseQuestionnaireApi.QuestionnaireExists(questionnaireName, serverParkName);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Failed to check if questionnaire {questionnaireName} exists on server park {serverParkName}. Error: {ex.Message}",
                    ex);
            }
        }

        private QuestionnaireStatusType GetQuestionnaireStatusSafe(string questionnaireName, string serverParkName)
        {
            try
            {
                return _blaiseQuestionnaireApi.GetQuestionnaireStatus(questionnaireName, serverParkName);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Failed to get status for questionnaire {questionnaireName} on server park {serverParkName}. Error: {ex.Message}",
                    ex);
            }
        }

        private string TryGetInstallDate(string questionnaireName, string serverParkName)
        {
            try
            {
                return GetQuestionnaireInstallDate(questionnaireName, serverParkName).ToString("O");
            }
            catch
            {
                return "(unavailable)";
            }
        }
    }
}
