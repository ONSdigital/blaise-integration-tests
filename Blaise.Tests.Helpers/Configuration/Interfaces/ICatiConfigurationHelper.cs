using System;

namespace Blaise.Tests.Helpers.Configuration.Interfaces
{
    public interface ICatiConfigurationHelper
    {
        string CatiAdminUsername { get; }
        string CatiAdminPassword { get; }
        string AdminRole { get; }

        string CatiInterviewUsername { get; }
        string CatiInterviewPassword { get; }
        string InterviewRole { get; }

        string CatiBaseUrl { get; }
        string LoginUrl { get; }
        string DayBatchUrl { get; }
        string SchedulerUrl { get; }
        string SpecificationUrl { get; }
        string SurveyUrl { get; }
        string CaseUrl { get; }
        string CaseInfoUrl { get; }
    }
}
