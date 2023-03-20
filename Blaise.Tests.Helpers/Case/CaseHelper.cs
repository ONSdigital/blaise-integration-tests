using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Models.Case;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

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
            DeleteCases();
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
            try
            {
                var cases = _blaiseCaseApi.GetCases(BlaiseConfigurationHelper.InstrumentName,
                    BlaiseConfigurationHelper.ServerParkName);

                while (!cases.EndOfSet)
                {
                    try
                    {
                        var primaryKey = _blaiseCaseApi.GetPrimaryKeyValue(cases.ActiveRecord);

                        _blaiseCaseApi.RemoveCase(primaryKey, BlaiseConfigurationHelper.InstrumentName,
                            BlaiseConfigurationHelper.ServerParkName);
                    }
                    catch (Exception)
                    {
                        /*Allows the code to continue gracefully and move to the next case to delete*/
                    }

                    cases.MoveNext();
                }
            }
            catch (StatNeth.Blaise.API.DataLink.DataLinkException)
            {
                /*Ignored as indicates there were no cases*/
            }
        }

        public int NumberOfCasesInInstrument()
        {
            try
            {
                return _blaiseCaseApi.GetNumberOfCases(BlaiseConfigurationHelper.InstrumentName,
                    BlaiseConfigurationHelper.ServerParkName);
            }
            catch (WebException ex)
            {
                // Handle network errors
                //Console.WriteLine($"Error getting number of cases: {ex.Message}");
                return 0;
            }
            catch (DataLinkException)
            {
                // Handle other exceptions
                //Console.WriteLine($"Unexpected error getting number of cases: {ex.Message}");
                return 0; /*This error constantly throws*/
            }
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
