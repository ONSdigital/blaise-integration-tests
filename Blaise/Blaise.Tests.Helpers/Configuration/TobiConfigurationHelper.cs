using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blaise.Tests.Helpers.Extensions;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class TobiConfigurationHelper
    {
        public static string TobiUrl => ConfigurationExtensions.GetVariable("TobiUrl");
        public static string SurveyUrl = $"{TobiUrl}/survey/{BlaiseConfigurationHelper.InstrumentName.Substring(0, 3)}";
    }
}
