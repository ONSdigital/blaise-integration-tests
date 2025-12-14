namespace Blaise.Tests.Helpers.Case
{
    using Blaise.Nuget.Api.Api;
    using Blaise.Tests.Helpers.Configuration;

    public static class CaseHelperExtensions
    {
        public static void DeleteCases(this CaseHelper caseHelper)
        {
            caseHelper.DeleteCases();
        }

        public static int NumberOfCasesInQuestionnaire(this CaseHelper caseHelper)
        {
            return caseHelper.NumberOfCasesInQuestionnaire();
        }
    }
}
