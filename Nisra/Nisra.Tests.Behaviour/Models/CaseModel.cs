using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Blaise.Nuget.Api.Contracts.Enums;
using Newtonsoft.Json;

namespace BlaiseNisraCaseProcessor.Tests.Behaviour.Models
{
    public class CaseModel
    {
        public CaseModel(int primaryKey, WebFormStatusType webFormStatusType, int hOut)
        {
            PrimaryKey = $"{primaryKey}";
            CaseData = BuildDefaultCaseData();
            CaseData["WebFormStatus"] = ((int) webFormStatusType).ToString();
            CaseData["QHAdmin.HOut"] = hOut.ToString();
            CaseData["serial_number"] = PrimaryKey;
        }

        public string PrimaryKey { get; set; }

        public Dictionary<string, string> CaseData { get; set; }

        private static Dictionary<string, string> BuildDefaultCaseData()
        {
            var dataFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Data");
            var caseData = GetCaseData(dataFolder);
            var fedForwardData = GetFedForwardData(dataFolder);

            //copy all entries in fed forward to case
            fedForwardData.ToList().ForEach(x => caseData.Add(x.Key, x.Value));

            return caseData;
        }

        private static Dictionary<string, string> GetCaseData(string dataFolder)
        {
            var jsonCaseDataFilePath = Path.Combine(dataFolder, "CaseData.json");

            using (var file = File.OpenText(jsonCaseDataFilePath))
            {
                var serializer = new JsonSerializer();
                var caseData = (Dictionary<string, string>)serializer.Deserialize(file, typeof(Dictionary<string, string>));

                return caseData;
            }
        }

        private static Dictionary<string, string> GetFedForwardData(string dataFolder)
        {
            var jsonFedForwardDataFilePath = Path.Combine(dataFolder, "FedForwardData.json");

            using (var file = File.OpenText(jsonFedForwardDataFilePath))
            {
                var serializer = new JsonSerializer();
                var caseData = (Dictionary<string, string>)serializer.Deserialize(file, typeof(Dictionary<string, string>));

                return caseData;
            }
        }
    }
}
