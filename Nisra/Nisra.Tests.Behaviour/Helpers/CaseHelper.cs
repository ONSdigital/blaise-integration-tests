﻿using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using Blaise.Nuget.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using BlaiseNisraCaseProcessor.Tests.Behaviour.Builders;
using BlaiseNisraCaseProcessor.Tests.Behaviour.Enums;
using BlaiseNisraCaseProcessor.Tests.Behaviour.Models;

namespace BlaiseNisraCaseProcessor.Tests.Behaviour.Helpers
{
    public class CaseHelper
    {
        private readonly IBlaiseApi _blaiseApi;

        private int _primaryKey;
        private readonly ConnectionModel _connectionModel;
        private readonly string _instrumentName;
        private readonly string _serverPark;

        public CaseHelper()
        {
            _blaiseApi = new BlaiseApi();
            _connectionModel = _blaiseApi.GetDefaultConnectionModel();
            _primaryKey = 900000;
            _instrumentName = ConfigurationManager.AppSettings["InstrumentName"];
            _serverPark = ConfigurationManager.AppSettings["ServerPark"];
        }

        public int CreateCase(string databaseFilePath, int outcome, ModeType mode)
        {
            var caseModel = new CaseModel(_primaryKey.ToString(), outcome.ToString(), mode);
            _blaiseApi.CreateNewDataRecord(databaseFilePath, caseModel.PrimaryKey, caseModel.BuildCaseData());

            return _primaryKey;
        }

        public void CreateCase(string databaseFilePath, int primaryKey, int outcome, ModeType mode)
        {
            var caseModel = new CaseModel(primaryKey.ToString(), outcome.ToString(), mode);
            _blaiseApi.CreateNewDataRecord(databaseFilePath, caseModel.PrimaryKey, caseModel.BuildCaseData());
        }

        public void CreateCases(string databaseFilePath, int numberOfCases, int outcome, ModeType mode)
        {
            for (var i = 0; i < numberOfCases; i++)
            {
                _primaryKey++;

                CreateCase(databaseFilePath, _primaryKey, outcome, mode);
            }
        }

        public void CreateCases(string databaseFilePath, IEnumerable<CaseModel> cases)
        {
            foreach (var caseModel in cases)
            {
                _blaiseApi.CreateNewDataRecord(databaseFilePath, caseModel.PrimaryKey, caseModel.BuildCaseData());
            }
        }

        public void CreateCasesInDatabase(int numberOfCases, int outcome, ModeType mode)
        {
            for (var i = 0; i < numberOfCases; i++)
            {
                _primaryKey++;
                CreateCaseInDatabase(_primaryKey, outcome, mode);
            }
        }

        public void CreateCaseInDatabase(int primaryKey, int outcome, ModeType mode)
        {
            var caseModel = new CaseModel($"{primaryKey}", outcome.ToString(), mode);
            _blaiseApi.CreateNewDataRecord(_connectionModel, $"{primaryKey}", caseModel.BuildBasicData(), _instrumentName, _serverPark);
        }

        public void CreateCasesInDatabase(IEnumerable<CaseModel> cases)
        {
            foreach (var caseModel in cases)
            {
                _blaiseApi.CreateNewDataRecord(_connectionModel, caseModel.PrimaryKey, caseModel.BuildBasicData(), _instrumentName, _serverPark);
            }
        }
        
        public int GetNumberOfCasesInDatabase()
        {
            return _blaiseApi.GetNumberOfCases(_connectionModel, _instrumentName, _serverPark);
        }

        public IEnumerable<CaseModel> GetCasesInDatabase()
        {
            var caseModels = new List<CaseModel>();

            var casesInDatabase = _blaiseApi.GetDataSet(_connectionModel, _instrumentName, _serverPark);

            while (!casesInDatabase.EndOfSet)
            {
                var caseRecord = casesInDatabase.ActiveRecord;

                var outcome = _blaiseApi.GetFieldValue(caseRecord, FieldNameType.HOut).IntegerValue.ToString(CultureInfo.InvariantCulture);
                var mode = _blaiseApi.GetFieldValue(caseRecord, FieldNameType.Mode).EnumerationValue;

                caseModels.Add(new CaseModel(_blaiseApi.GetPrimaryKeyValue(caseRecord), outcome, (ModeType)mode));
                casesInDatabase.MoveNext();
            }

            return caseModels;
        }

        public CaseModel GetCaseInDatabase(int primaryKey)
        {
            var blaiseCaseRecord = _blaiseApi.GetDataRecord(_connectionModel, primaryKey.ToString(), _instrumentName, _serverPark);
            var outcome = _blaiseApi.GetFieldValue(blaiseCaseRecord, FieldNameType.HOut).IntegerValue.ToString(CultureInfo.InvariantCulture);
            var mode = _blaiseApi.GetFieldValue(blaiseCaseRecord, FieldNameType.Mode).EnumerationValue;

            return new CaseModel(primaryKey.ToString(), outcome, (ModeType)mode);
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
    }
}
