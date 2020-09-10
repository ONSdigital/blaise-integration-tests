using System.Configuration;
using Blaise.Nuget.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using BlaiseNisraCaseProcessor.Tests.Behaviour.Models;

namespace BlaiseNisraCaseProcessor.Tests.Behaviour.Helpers
{
    public class CaseHelper
    {
        private readonly IBlaiseApi _blaiseApi;

        private int _primaryKey;
        private string _instrumentName;
        private string _serverPark;

        public CaseHelper()
        {
            _blaiseApi = new BlaiseApi();
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
            return _blaiseApi.GetNumberOfCases(
                _blaiseApi.GetDefaultConnectionModel(),
                _instrumentName, _serverPark);
        }
    }
}
