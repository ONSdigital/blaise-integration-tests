namespace Blaise.Cati.Tests.Behaviour.Steps
{
    using System.Collections.Generic;
    using Blaise.Tests.Helpers.Cati;
    using Blaise.Tests.Models.Case;
    using NUnit.Framework;
    using Reqnroll;

    [Binding]
    public sealed class DaybatchSteps
    {
        [Then(@"the sample cases are present on the daybatch page")]
        public void ThenTheSampleCasesArePresentOnTheDaybatchPage(IEnumerable<CaseModel> cases)
        {
            var entriesText = CatiManagementHelper.GetInstance().GetDaybatchEntriesText();

            Assert.That(
                entriesText,
                Is.Not.Null.And.Not.Empty,
                "The daybatch entries text should not be null or empty");
        }
    }
}
