using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Smoke.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaise.Smoke.Tests.Helpers
{
    public class CaseHelper
    {
        private readonly IBlaiseCaseApi _blaiseCaseApi;
        private readonly ConfigurationHelper _configurationHelper;

        public CaseHelper()
        {
            _blaiseCaseApi = new BlaiseCaseApi();
            _configurationHelper = new ConfigurationHelper();         
        }

        public void CreateCases(string instrumentName, IEnumerable<CaseModel> specficCases)
        {
            foreach (var specficCase in specficCases)
            {
                CreateCasesInAnInstrument(instrumentName, specficCase);
            }
        }

        public void CreateCasesInAnInstrument(string instrumentName, CaseModel model)
        {        
            _blaiseCaseApi.CreateNewDataRecord(model.PrimaryKey, model.ConvertToDictionary(), instrumentName, _configurationHelper.ServerParkName);
        }

        public bool GetCasesFromAnInstrument(string instrumentName, IEnumerable<CaseModel> originalCases)
        {
            List<CaseModel> caseModelList = new List<CaseModel>();
            var cases = _blaiseCaseApi.GetDataSet(instrumentName, _configurationHelper.ServerParkName);
            while (!cases.EndOfSet)
            {
                caseModelList.Add(
                    new CaseModel { 
                        PrimaryKey = _blaiseCaseApi.GetPrimaryKeyValue(cases.ActiveRecord),
                        Outcome = _blaiseCaseApi.GetFieldValue(cases.ActiveRecord,FieldNameType.HOut).ValueAsText,
                        TelNo = "07000 000 00"
                        
                    });
                cases.MoveNext();
            }
            return originalCases.SequenceEqual(caseModelList);
        }

    }
}
