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
        private readonly BlaiseConfigurationHelper _configurationHelper;

        public CaseHelper()
        {
            _blaiseCaseApi = new BlaiseCaseApi();
            _configurationHelper = new BlaiseConfigurationHelper();
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
            _blaiseCaseApi.CreateNewDataRecord(caseModel.PrimaryKey, caseModel.FieldData(), _configurationHelper.InstrumentName, _configurationHelper.ServerParkName);
        }

        public void DeleteCases()
        {
            var cases = _blaiseCaseApi.GetDataSet(_configurationHelper.InstrumentName, _configurationHelper.ServerParkName);

            while (!cases.EndOfSet)
            {
                var primaryKey = _blaiseCaseApi.GetPrimaryKeyValue(cases.ActiveRecord);

                _blaiseCaseApi.RemoveCase(primaryKey, _configurationHelper.InstrumentName, _configurationHelper.ServerParkName);

                cases.MoveNext();
            }
        }

        public int NumberOfCasesInBlaise()
        {
            return _blaiseCaseApi.GetNumberOfCases(_configurationHelper.InstrumentName,
                _configurationHelper.ServerParkName);
        }
        
        public IEnumerable<CaseModel> GetCasesInBlaise()
        {
            var caseModels = new List<CaseModel>();
            var casesInDatabase = _blaiseCaseApi.GetDataSet(_configurationHelper.InstrumentName, _configurationHelper.ServerParkName);

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
    }
}
