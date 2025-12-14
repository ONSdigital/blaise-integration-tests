namespace Blaise.Tests.Helpers.Case
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Interfaces;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Models.Case;
    using StatNeth.Blaise.API.DataLink;
    using StatNeth.Blaise.API.DataRecord;

    public class CaseHelper
    {
        private static CaseHelper _currentInstance;
        private readonly IBlaiseCaseApi _blaiseCaseApi;

        public CaseHelper()
        {
            _blaiseCaseApi = new BlaiseCaseApi();
        }

        public static CaseHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new CaseHelper());
        }

        public void CreateCases(IEnumerable<CaseModel> caseModels)
        {
            foreach (var caseModel in caseModels)
            {
                CreateCase(caseModel);
            }
        }

        public void CreateCase(CaseModel caseModel)
        {
            CreateCaseWithRetry(caseModel.PrimaryKeyValues, caseModel.FieldData());
        }

        public void CreateCase()
        {
            var caseModel = BuildDefaultCase();
            CreateCaseWithRetry(caseModel.PrimaryKeyValues, caseModel.FieldData());
        }

        public void DeleteCases()
        {
            try
            {
                var cases = _blaiseCaseApi.GetCases(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);

                while (!cases.EndOfSet)
                {
                    try
                    {
                        var primaryKey = _blaiseCaseApi.GetPrimaryKeyValues(cases.ActiveRecord);

                        _blaiseCaseApi.RemoveCase(primaryKey, BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Warning: Failed to remove case. Error: {ex.Message}");
                    }

                    cases.MoveNext();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not retrieve cases to delete. Error: {ex.Message}");
            }
        }

        public int NumberOfCasesInQuestionnaire()
        {
            try
            {
                return _blaiseCaseApi.GetNumberOfCases(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Failed to retrieve case count. Defaulting to 0. Error: {ex.Message}");
                return 0;
            }
        }

        public IEnumerable<CaseModel> GetCasesInBlaise()
        {
            var caseModels = new List<CaseModel>();
            var casesInDatabase = _blaiseCaseApi.GetCases(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);

            while (!casesInDatabase.EndOfSet)
            {
                caseModels.Add(MapRecordToCaseModel(casesInDatabase.ActiveRecord));
                casesInDatabase.MoveNext();
            }

            return caseModels;
        }

        private void CreateCaseWithRetry(Dictionary<string, string> primaryKeyValues, Dictionary<string, string> fieldData)
        {
            var retries = 3;
            const int DelayInMs = 1000;

            while (retries > 0)
            {
                try
                {
                    _blaiseCaseApi.CreateCase(primaryKeyValues, fieldData, BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
                    return;
                }
                catch (DataLinkException ex)
                {
                    if (ex.Message.ToLower().Contains("already locked"))
                    {
                        retries--;
                        if (retries == 0)
                        {
                            throw;
                        }

                        Console.WriteLine($"Case is locked, retrying in {DelayInMs}ms ({retries} retries remaining)");
                        Thread.Sleep(DelayInMs);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private CaseModel MapRecordToCaseModel(IDataRecord caseRecord)
        {
            var primaryKey = _blaiseCaseApi.GetPrimaryKeyValues(caseRecord);
            var outcomeCode = _blaiseCaseApi.GetFieldValue(caseRecord, FieldNameType.HOut).IntegerValue.ToString(CultureInfo.InvariantCulture);
            var telephoneNumber = _blaiseCaseApi.GetFieldValue(caseRecord, FieldNameType.TelNo).IntegerValue.ToString(CultureInfo.InvariantCulture);

            return new CaseModel(primaryKey, outcomeCode, telephoneNumber);
        }

        private CaseModel BuildDefaultCase()
        {
            var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "9001" } };
            return new CaseModel(primaryKeyValues, "110", "07000000000");
        }
    }
}
