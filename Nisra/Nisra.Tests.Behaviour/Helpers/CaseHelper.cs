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

        public CaseHelper()
        {
            _blaiseApi = new BlaiseApi();
            _primaryKey = 900000;
        }

        public void CreateCase(string databaseFilePath, int primaryKey, 
            WebFormStatusType webFormStatusType, int hOut)
        {
            var caseModel = new CaseModel(primaryKey, webFormStatusType, hOut);
            _blaiseApi.CreateNewDataRecord(databaseFilePath, caseModel.PrimaryKey, caseModel.CaseData);
        }

        private void CreateCases(string databaseFilePath, int numberOfCases, 
            WebFormStatusType status, int outcome)
        {
            for (var i = 0; i < numberOfCases; i++)
            {
                _primaryKey++;

                CreateCase(databaseFilePath, _primaryKey, status, outcome);
            }
        }
    }
}
