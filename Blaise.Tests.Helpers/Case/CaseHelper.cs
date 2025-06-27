using System;
using System.Collections.Generic;
using System.Globalization;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Models.Case;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Tests.Helpers.Case
{
    public class CaseHelper
    {
        private readonly IBlaiseCaseApi _blaiseCaseApi;

        private static CaseHelper _currentInstance;

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
            _blaiseCaseApi.CreateCase(caseModel.PrimaryKeyValues, caseModel.FieldData(), BlaiseConfigurationHelper.QuestionnaireName,
                BlaiseConfigurationHelper.ServerParkName);
        }

        public void CreateCase()
        {
            var caseModel = BuildDefaultCase();
            _blaiseCaseApi.CreateCase(caseModel.PrimaryKeyValues, caseModel.FieldData(), BlaiseConfigurationHelper.QuestionnaireName,
                BlaiseConfigurationHelper.ServerParkName);
        }

        public void DeleteCases()
        {
            try
            {
                var cases = _blaiseCaseApi.GetCases(
                    BlaiseConfigurationHelper.QuestionnaireName,
                    BlaiseConfigurationHelper.ServerParkName);

                while (!cases.EndOfSet)
                {
                    try
                    {
                        var primaryKey = _blaiseCaseApi.GetPrimaryKeyValues(cases.ActiveRecord);

                        _blaiseCaseApi.RemoveCase(primaryKey, BlaiseConfigurationHelper.QuestionnaireName,
                            BlaiseConfigurationHelper.ServerParkName);
                    }
                    catch (Exception)
                    {
                        /*Ignored - better to implement ILogger*/
                    }

                    cases.MoveNext();
                }
            }
            catch (Exception)
            {
                /*Ignored - better to implement ILogger*/
            }
        }

        public int NumberOfCasesInQuestionnaire()
        {
            try
            {
                return _blaiseCaseApi.GetNumberOfCases(
                    BlaiseConfigurationHelper.QuestionnaireName,
                    BlaiseConfigurationHelper.ServerParkName);
            }
            catch (Exception)
            {
                // Could be improved by implementing ILogger
                return 0;
            }
        }

        public IEnumerable<CaseModel> GetCasesInBlaise()
        {
            var caseModels = new List<CaseModel>();
            var casesInDatabase = _blaiseCaseApi.GetCases(
                BlaiseConfigurationHelper.QuestionnaireName,
                BlaiseConfigurationHelper.ServerParkName);

            while (!casesInDatabase.EndOfSet)
            {
                caseModels.Add(MapRecordToCaseModel(casesInDatabase.ActiveRecord));
                casesInDatabase.MoveNext();
            }

            return caseModels;
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
