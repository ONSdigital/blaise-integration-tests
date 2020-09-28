using System.Collections.Generic;
using System.Configuration;
using Blaise.Api.Tests.Behaviour.Models;
using Blaise.Nuget.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Api.Tests.Behaviour.Helpers
{
    public class DataHelper
    {
        private readonly IBlaiseApi _blaiseApi;

        private readonly ConnectionModel _connectionModel;
        private readonly string _instrumentName;
        private readonly string _serverPark;

        private readonly string _defaultPrimaryKey;
        private readonly Dictionary<string, string> _defaultFieldData;
        private readonly int _defaultCaseId;

        public DataHelper()
        {
            _blaiseApi = new BlaiseApi();

            _connectionModel = _blaiseApi.GetDefaultConnectionModel();
            _instrumentName = ConfigurationManager.AppSettings["InstrumentName"];
            _serverPark = ConfigurationManager.AppSettings["ServerPark"];

            _defaultPrimaryKey = "900000";
            _defaultFieldData = new Dictionary<string, string>
            {
                {FieldNameType.CaseId.FullName(), $"{_defaultCaseId}"}
            };

            _defaultCaseId = 1;
        }

        public CaseModel BuildBasicCase()
        {
            return new CaseModel
            {
                PrimaryKey = _defaultPrimaryKey,
                FieldData = _defaultFieldData
            };
        }

        public CaseModel BuildCase(string primaryKey)
        {
            return new CaseModel
            {
                PrimaryKey = primaryKey,
                FieldData = _defaultFieldData
            };
        }
        public CaseModel BuildCase(Dictionary<string, string> fieldData)
        {
            return new CaseModel
            {
                PrimaryKey = _defaultPrimaryKey,
                FieldData = fieldData
            };
        }

        public void CreateCase(CaseModel caseModel)
        {
            _blaiseApi.CreateNewDataRecord(_connectionModel, caseModel.PrimaryKey, caseModel.FieldData, _instrumentName, _serverPark);
        }

        public void UpdateCase(string primaryKey, Dictionary<string, string> fieldData)
        {
            var dataRecord = _blaiseApi.GetDataRecord(_connectionModel, primaryKey, _instrumentName, _serverPark);

            _blaiseApi.UpdateDataRecord(_connectionModel, dataRecord, fieldData, _instrumentName, _serverPark);
        }

        public void DeleteCase(string primaryKey)
        {
            _blaiseApi.RemoveCase(_connectionModel, primaryKey, _instrumentName, _serverPark);
        }

        public bool CaseExists(string primaryKey)
        {
            return _blaiseApi.CaseExists(_connectionModel, primaryKey, _instrumentName, _serverPark);
        }

        public CaseModel GetCase(string primaryKey)
        {
            var dataRecord = _blaiseApi.GetDataRecord(_connectionModel, primaryKey, _instrumentName, _serverPark);
            var fieldData = MapFieldDictionaryFromRecordFields((IDataRecord2)dataRecord);

            return new CaseModel
            {
                PrimaryKey = primaryKey,
                FieldData = fieldData
            };
        }

        public List<CaseModel> GetCases()
        {
            var caseModelList = new List<CaseModel>();
            var cases = _blaiseApi.GetDataSet(_connectionModel, _instrumentName, _serverPark);

            while (!cases.EndOfSet)
            {
                caseModelList.Add(new CaseModel { PrimaryKey = _blaiseApi.GetPrimaryKeyValue(cases.ActiveRecord) });

                cases.MoveNext();
            }

            return caseModelList;
        }

        public void DeleteCasesInDatabase()
        {
            var cases = _blaiseApi.GetDataSet(_connectionModel, _instrumentName, _serverPark);

            while (!cases.EndOfSet)
            {
                var primaryKey = _blaiseApi.GetPrimaryKeyValue(cases.ActiveRecord);

                _blaiseApi.RemoveCase(_connectionModel, primaryKey,
                    _instrumentName, _serverPark);

                cases.MoveNext();
            }
        }

        private Dictionary<string, string> MapFieldDictionaryFromRecordFields(IDataRecord2 recordData)
        {
            var fieldDictionary = new Dictionary<string, string>();
            var dataFields = recordData.GetDataFields();

            foreach (var dataField in dataFields)
            {
                fieldDictionary[dataField.FullName] = dataField.DataValue.ValueAsText;
            }

            return fieldDictionary;
        }
    }
}
