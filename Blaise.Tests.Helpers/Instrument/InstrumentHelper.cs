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
            return _blaiseQuestionnaireApi.GetQuestionnaireStatus(BlaiseConfigurationHelper.QuestionnaireName,
                BlaiseConfigurationHelper.ServerParkName);
        }

        public void CheckIfInstrumentIsErroneous(string questionnaireName)
        {
            try
            {
                var questionnaireStatus = GetQuestionnaireStatus();

                if (questionnaireStatus == QuestionnaireStatusType.Erroneous)
                {
                    Console.WriteLine($"InstrumentHelper CheckIfInstrumentIsErroneous :Questionnaire {questionnaireName} SEEMS to be erroneous. Check on the management node if the tests continue to fail");
                    return;
                }

                Console.WriteLine($"InstrumentHelper CheckIfInstrumentIsErroneous :Questionnaire {questionnaireName} is not erroneous, it is in the state {questionnaireStatus}");
            }
            catch (DataNotFoundException)
            {
                Console.WriteLine($"InstrumentHelper CheckIfInstrumentIsErroneous :Questionnaire {questionnaireName} does not exist");
            }
        }

        public static string InstrumentPackagePath(string questionnairePath, string questionnaireName)
        {
            return $"{questionnairePath}//{questionnaireName}.bpkg";
        }

        public void InstallInstrument()
        {
            InstallInstrument(BlaiseConfigurationHelper.QuestionnaireName);
        }

        public void InstallInstrument(string questionnaireName)
        {
            Console.WriteLine($"InstrumentHelper InstallInstrument: Questionnaire {BlaiseConfigurationHelper.QuestionnaireName} is about to be installed");
            var questionnairePackage = InstrumentPackagePath(BlaiseConfigurationHelper.QuestionnairePath, questionnaireName);

            Console.WriteLine($"InstrumentHelper InstallInstrument: install Questionnaire {BlaiseConfigurationHelper.QuestionnaireName}");
            _blaiseQuestionnaireApi.InstallQuestionnaire(questionnaireName,
                BlaiseConfigurationHelper.ServerParkName,
                questionnairePackage,
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
            Console.WriteLine($"InstrumentHelper UninstallSurvey: Removing questionnaire {BlaiseConfigurationHelper.QuestionnaireName}");
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
            Console.WriteLine($"InstrumentHelper SurveyExists: Check to see if questionnaire {BlaiseConfigurationHelper.QuestionnaireName} has been installed");
            var counter = 0;
            const int maxCount = 10;

            while (!_blaiseQuestionnaireApi.QuestionnaireExists(questionnaireName, BlaiseConfigurationHelper.ServerParkName))
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
            Console.WriteLine($"InstrumentHelper SurveyExists: Questionnaire {BlaiseConfigurationHelper.QuestionnaireName} has been installed");

            return true;
        }

        private bool SurveyNoLongerExists(string questionnaireName, int timeoutInSeconds)
        {
            Console.WriteLine($"InstrumentHelper SurveyNoLongerExists: Check to see if questionnaire {BlaiseConfigurationHelper.QuestionnaireName} has been removed");
            var counter = 0;
            const int maxCount = 10;

            while (_blaiseQuestionnaireApi.QuestionnaireExists(questionnaireName, BlaiseConfigurationHelper.ServerParkName))
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

            Console.WriteLine($"InstrumentHelper SurveyNoLongerExists: Questionnaire {BlaiseConfigurationHelper.QuestionnaireName} has been removed");
            return true;
        }
    }
}
