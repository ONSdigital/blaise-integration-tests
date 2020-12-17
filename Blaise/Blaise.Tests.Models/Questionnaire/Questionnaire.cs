using System;
using Blaise.Nuget.Api.Contracts.Enums;

namespace Blaise.Tests.Models.Questionnaire
{
    public class Questionnaire
    {
        public string Name { get; set; }
        public string ServerParkName { get; set; }
        public DateTime InstallDate { get; set; }
        public SurveyStatusType Status { get; set; }
    }
}
