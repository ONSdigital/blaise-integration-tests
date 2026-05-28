namespace Blaise.Tests.Helpers.Framework
{
    using System;
    using Blaise.Tests.Helpers.Questionnaire;

    public static class TestDiagnosticsHelper
    {
        public static void LogBlaisePreflight(string questionnaireName, string serverParkName)
        {
            Console.WriteLine("=== Blaise preflight ===");
            Console.WriteLine($"Server park: {serverParkName}");
            Console.WriteLine($"Questionnaire: {questionnaireName}");

            var exists = QuestionnaireHelper.GetInstance().CheckQuestionnaireExists(questionnaireName, serverParkName);
            Console.WriteLine($"Questionnaire exists: {exists}");

            if (exists)
            {
                var status = QuestionnaireHelper.GetInstance().GetQuestionnaireStatus(questionnaireName, serverParkName);
                Console.WriteLine($"Questionnaire status: {status}");
                var installDate = QuestionnaireHelper.GetInstance().GetQuestionnaireInstallDate(questionnaireName, serverParkName);
                Console.WriteLine($"Questionnaire install date: {installDate:O}");
            }

            Console.WriteLine("=== End Blaise preflight ===");
        }
    }
}
