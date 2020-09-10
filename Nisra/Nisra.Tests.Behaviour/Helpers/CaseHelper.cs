using System.Configuration;
using Blaise.Nuget.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using BlaiseNisraCaseProcessor.Tests.Behaviour.Models;
using StatNeth.Blaise.API.DataRecord;

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

        public void CreateCase(string databaseFilePath, int primaryKey, 
            WebFormStatusType webFormStatusType, int hOut)
        {
            var caseModel = new CaseModel(primaryKey, webFormStatusType, hOut);
            _blaiseApi.CreateNewDataRecord(databaseFilePath, caseModel.PrimaryKey, caseModel.CaseData);
        }

        public void CreateCases(string databaseFilePath, int numberOfCases, 
            WebFormStatusType status, int outcome)
        {
            for (var i = 0; i < numberOfCases; i++)
            {
                _primaryKey++;

                CreateCase(databaseFilePath, _primaryKey, status, outcome);
            }
        }

        public int GetNumberOfCasesInDatabase()
        {
            return _blaiseApi.GetNumberOfCases(_connectionModel, _instrumentName, _serverPark);
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
