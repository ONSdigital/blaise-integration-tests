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
            _blaiseCaseApi.CreateCase(caseModel.PrimaryKey, caseModel.FieldData(), BlaiseConfigurationHelper.InstrumentName, 
                BlaiseConfigurationHelper.ServerParkName);
        }

        public void CreateCase()
        {
            var caseModel = BuildDefaultCase();
            _blaiseCaseApi.CreateCase(caseModel.PrimaryKey, caseModel.FieldData(), BlaiseConfigurationHelper.InstrumentName,
                BlaiseConfigurationHelper.ServerParkName);
        }

        public void DeleteCases()
        {
            var cases = _blaiseCaseApi.GetCases(BlaiseConfigurationHelper.InstrumentName, 
                BlaiseConfigurationHelper.ServerParkName);

            while (!cases.EndOfSet)
            {
                var primaryKey = _blaiseCaseApi.GetPrimaryKeyValue(cases.ActiveRecord);

                _blaiseCaseApi.RemoveCase(primaryKey, BlaiseConfigurationHelper.InstrumentName, 
                    BlaiseConfigurationHelper.ServerParkName);

                cases.MoveNext();
            }
        }

        public int NumberOfCasesInInstrument()
        {
            return _blaiseCaseApi.GetNumberOfCases(BlaiseConfigurationHelper.InstrumentName, 
                BlaiseConfigurationHelper.ServerParkName);
        }
        
        public IEnumerable<CaseModel> GetCasesInBlaise()
        {
            var caseModels = new List<CaseModel>();
            var casesInDatabase = _blaiseCaseApi.GetCases(BlaiseConfigurationHelper.InstrumentName, 
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
            var primaryKey = _blaiseCaseApi.GetPrimaryKeyValue(caseRecord);
            var outcomeCode = _blaiseCaseApi.GetFieldValue(caseRecord, FieldNameType.HOut).IntegerValue.ToString(CultureInfo.InvariantCulture);
            var telephoneNumber = _blaiseCaseApi.GetFieldValue(caseRecord, FieldNameType.TelNo).IntegerValue.ToString(CultureInfo.InvariantCulture);

            return new CaseModel(primaryKey, outcomeCode, telephoneNumber);
        }

        private CaseModel BuildDefaultCase()
        {
            return new CaseModel("900001", "110", "07000000000");
        }
    }
}
